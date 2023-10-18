<?php

namespace App\Service;

use App\Dto\MedicalRecord\RequestMedicalRecordDto;
use App\Dto\MedicalRecord\ResponseMedicalRecordDto;
use App\Entity\MedicalRecord;
use App\Entity\Patient;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\AppointmentRepository;
use App\Repository\DoctorRepository;
use App\Repository\MedicalRecordRepository;
use App\Repository\PatientRepository;
use App\Repository\SpecializationRepository;
use App\Transformer\MedicalRecord\MedicalRecordResponseDtoTransformer;
use App\Transformer\Paginator\PaginatorResponseTransformer;

class MedicalRecordService
{
    const ITEM_PER_PAGE = 10;

    public function __construct(
        private readonly MedicalRecordRepository $medicalRecordRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly PatientRepository $patientRepository,
        private readonly AppointmentRepository $appointmentRepository,
        private readonly SpecializationRepository $specializationRepository,
        private readonly MedicalRecordResponseDtoTransformer $medicalRecordResponseDtoTransformer,
        private readonly PaginatorResponseTransformer $paginatorResponseTransformer
    ) {}

    /**
     * @throws NotFoundException
     */
    private function findPatient($email): Patient
    {
        $patient = $this->patientRepository->findOneByEmail($email);
        if (is_null($patient))
            throw new NotFoundException('Patient does not exist');
        return $patient;
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     */
    public function create(string $doctorIdentifier, RequestMedicalRecordDto $dto): ResponseMedicalRecordDto
    {
        // find out if the doctor exists
        $doctor = $this->doctorRepository->findOneByEmail($doctorIdentifier);
        if (is_null($doctor))
            throw new NotFoundException('Doctor does not exist');

        // find patient or throw
        $patient = $this->findPatient($dto->getPatientEmail());

        // find out if the appointment exists and check medical record
        $appointment = $this->appointmentRepository->findOneById($dto->getAppointmentId());
        if (is_null($appointment))
            throw new NotFoundException('Appointment does not exist');
        if ($this->medicalRecordRepository->doesAppointmentExist($appointment->getId()))
            throw new AlreadyExistException('Appointment already have medical record');

        // find out if the specialization exists
        $specialization = $this->specializationRepository->findOneById($dto->getSpecializationId());
        if (is_null($specialization))
            throw new NotFoundException('Specialization does not exist');

        // check if the doctor has the given specialization
        if (!$specialization->getDoctors()->contains($doctor))
            throw new NotFoundException('Doctor does not have this specialization');

        $record = (new MedicalRecord())
            ->setTitle($dto->getTitle())
            ->setDescription($dto->getDescription())
            ->setDoctorNote($dto->getDoctorNote())
            ->setAppointment($appointment)
            ->setPatient($patient)
            ->setDoctor($doctor)
            ->setSpecialization($specialization);

        $this->medicalRecordRepository->save($record, true);
        return $this->medicalRecordResponseDtoTransformer->transformFromObject($record);
    }

    /**
     * @throws NotFoundException
     */
    public function showForPatient(string $patientIdentifier, int $page): array
    {
        $patient = $this->findPatient($patientIdentifier);

        $result = $this->medicalRecordRepository
            ->findByPatientIdWithPagination($patient->getId(), $page, self::ITEM_PER_PAGE);
        $totalPages = $result['totalPages'];
        $medicalRecords = $result['medicalRecords'];

        return $this->paginatorResponseTransformer
            ->transformToArray($this->medicalRecordResponseDtoTransformer
                ->transformFromObjects($medicalRecords), $page, $totalPages);
    }
}