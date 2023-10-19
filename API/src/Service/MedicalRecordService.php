<?php

namespace App\Service;

use App\Dto\MedicalRecord\RequestMedicalRecordDto;
use App\Dto\MedicalRecord\ResponseMedicalRecordDto;
use App\Entity\Doctor\Doctor;
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
use Doctrine\ORM\NonUniqueResultException;

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
     * @throws AlreadyExistException
     * @throws NonUniqueResultException
     */
    public function create(string $doctorIdentifier, RequestMedicalRecordDto $dto): ResponseMedicalRecordDto
    {
        // find doctor or throw
        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);
        // find patient or throw
        $patient = $this->patientRepository->findOneByEmailOrThrow($dto->getPatientEmail());
        // find appointment or throw
        $appointment = $this->appointmentRepository->findOneByIdOrThrow($dto->getAppointmentId());
        // check if a medical record already exists
        if ($this->medicalRecordRepository->doesAppointmentExist($appointment->getId()))
            throw new AlreadyExistException('Appointment already has a medical record');
        // find specialization or throw
        $specialization = $this->specializationRepository->findOneByIdOrThrow($dto->getSpecializationId());
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
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function showForPatientOrDoctor(string $userIdentifier, int $page, string $userType): array
    {
        if ($userType !== 'patient' && $userType !== 'doctor')
            return [];

        if ($userType === 'doctor')
            $user = $this->doctorRepository->findOneByEmailOrThrow($userIdentifier);
        if ($userType === 'patient')
            $user = $this->patientRepository->findOneByEmailOrThrow($userIdentifier);

        $result = $this->medicalRecordRepository
            ->findByPatientIdOrDoctorIdWithPagination(
                $user->getId(), $page, self::ITEM_PER_PAGE, $userType);
        $totalPages = $result['totalPages'];
        $medicalRecords = $result['medicalRecords'];

        return $this->paginatorResponseTransformer
            ->transformToArray($this->medicalRecordResponseDtoTransformer
                ->transformFromObjects($medicalRecords), $page, $totalPages);
    }
}