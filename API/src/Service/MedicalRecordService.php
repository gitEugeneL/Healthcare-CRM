<?php

namespace App\Service;

use App\Dto\MedicalRecord\CreateMedicalRecordDto;
use App\Dto\MedicalRecord\ResponseMedicalRecordDto;
use App\Dto\MedicalRecord\UpdateMedicalRecordDto;
use App\Entity\MedicalRecord;
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
use Symfony\Component\Finder\Exception\AccessDeniedException;

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
        private readonly PaginatorResponseTransformer $paginatorResponseTransformer,
    ) {}

    private function createPaginationResponse(array $result, int $page): array
    {
        $totalPages = $result['totalPages'];
        $medicalRecords = $result['medicalRecords'];
        return  $this->paginatorResponseTransformer
            ->transformToArray($this->medicalRecordResponseDtoTransformer
                ->transformFromObjects($medicalRecords), $page, $totalPages);
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     * @throws NonUniqueResultException
     */
    public function create(string $doctorIdentifier, CreateMedicalRecordDto $dto): ResponseMedicalRecordDto
    {
        // find doctor or throw
        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);
        // find patient or throw
        $patient = $this->patientRepository->findOneByEmailOrThrow($dto->getPatientEmail());
        // find appointment or throw
        $appointment = $this->appointmentRepository->findOneByIdOrThrow($dto->getAppointmentId());
        // check if a medical record already exists
        if ($this->medicalRecordRepository->doesMedicalRecordExist($appointment->getId()))
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
    public function showForDoctor(string $doctorIdentifier,  int $patientId, int $page): array
    {
        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);
        $patient = $this->patientRepository->findOneByIdOrThrow($patientId);
        $result = $this->medicalRecordRepository
            ->findForDoctorIdWithPagination($doctor->getId(), $patient->getId(), $page, self::ITEM_PER_PAGE);
        return $this->createPaginationResponse($result, $page);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function showForPatient(string $patientIdentifier, int $page): array
    {
        $patient = $this->patientRepository->findOneByEmailOrThrow($patientIdentifier);
        $result = $this->medicalRecordRepository
            ->findForPatientIdWithPagination($patient->getId(), $page, self::ITEM_PER_PAGE);
        return $this->createPaginationResponse($result, $page);
    }

    /**
     * @throws NotFoundException
     */
    public function showOneForDoctor(int $medicalRecordId): ResponseMedicalRecordDto
    {
        $medicalRecord = $this->medicalRecordRepository->findOneByIdOrThrow($medicalRecordId);
        return $this->medicalRecordResponseDtoTransformer->transformFromObject($medicalRecord);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function showOneForPatient(string $patientIdentifier, int $medicalRecordId): ResponseMedicalRecordDto
    {
        $patient = $this->patientRepository->findOneByEmailOrThrow($patientIdentifier);
        $medicalRecord = $this->medicalRecordRepository->findOneByIdForPatientOrThrow($medicalRecordId, $patient->getId());
        return $this->medicalRecordResponseDtoTransformer->transformFromObject($medicalRecord);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function update(string $doctorIdentifier, int $medicalRecordId, UpdateMedicalRecordDto $dto): ResponseMedicalRecordDto
    {
        if (!$dto->getTitle() && !$dto->getDescription() && !$dto->getDoctorNote())
            throw new NotFoundException('Nothing to change');

        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);
        $medicalRecord = $this->medicalRecordRepository->findOneByIdOrThrow($medicalRecordId);
        if ($medicalRecord->getDoctor() !== $doctor)
            throw new AccessDeniedException("Doctor doesn't have access to update this medical record");

        if (!is_null($dto->getTitle()))
            $medicalRecord->setTitle($dto->getTitle());
        if (!is_null($dto->getDescription()))
            $medicalRecord->setDescription($dto->getDescription());
        if (!is_null($dto->getDoctorNote()))
            $medicalRecord->setDoctorNote($dto->getDoctorNote());
        $this->medicalRecordRepository->save($medicalRecord, true);
        return $this->medicalRecordResponseDtoTransformer->transformFromObject($medicalRecord);
    }
}