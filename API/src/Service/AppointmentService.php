<?php

namespace App\Service;

use App\Dto\Appointment\RequestAppointmentDto;
use App\Dto\Appointment\ResponseAppointmentDto;
use App\Entity\Appointment;
use App\Entity\Doctor\Doctor;
use App\Entity\Doctor\Status;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Repository\AppointmentRepository;
use App\Repository\DoctorRepository;
use App\Repository\PatientRepository;
use App\Transformer\Appointment\AppointmentResponseDtoTransformer;
use DateTime;
use Exception;

class AppointmentService
{
    public function __construct(
        private readonly AppointmentRepository $appointmentRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly PatientRepository $patientRepository,
        private readonly AppointmentResponseDtoTransformer $appointmentResponseDtoTransformer
    ) {}

    /**
     * @throws NotFoundException
     */
    private function findDoctor(int $doctorId): Doctor
    {
        $doctor = $this->doctorRepository->findOneById($doctorId);
        if (is_null($doctor) || $doctor->getStatus() === Status::DISABLED)
            throw new NotFoundException("Doctor does not exist or doctor is inactive");
        return $doctor;
    }

    /**
     * @throws Exception
     */
    private function findFreeDoctorHours(DateTime $date, Doctor $doctor): array
    {
        $doctorConfig = $doctor->getDoctorConfig();
        return $this->appointmentRepository
            ->findFreeHours(
                $doctor->getId(),
                $date,
                $doctorConfig->getStartTime(),
                $doctorConfig->getEndTime(),
                $doctorConfig->getInterval()
            );
    }

    private function checkWorkday(DateTime $date, array $doctorWorkdays): bool
    {
        $day = $date->format('N');
        return !in_array($day, $doctorWorkdays);
    }

    /**
     * @throws Exception
     */
    private function isDoctorAvailableOnDay(DateTime $date, DateTime $startTime, Doctor $doctor): bool
    {
        $freeDoctorHours = $this->findFreeDoctorHours($date, $doctor);
        return (in_array($startTime->format('H:i'), array_column($freeDoctorHours, 'start')));
    }

    private function createEndTime(DateTime $startTime, string $doctorInterval): DateTime
    {
        $intervalValue = intval($doctorInterval);
        $intervalUnit = substr($doctorInterval, -1);

        $endTime = clone $startTime;
        if ($intervalUnit === 'H')
            $endTime->modify("+{$intervalValue} hours");
        elseif ($intervalUnit === 'M')
            $endTime->modify("+{$intervalValue} minutes");
        return $endTime;
    }

    private function checkDatePattern(string $dateString): bool
    {
        $pattern = '/^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/';
        return preg_match($pattern, $dateString);
    }

    /**
     * @throws NotFoundException
     * @throws Exception
     */
    public function showFreeHours(RequestAppointmentDto $dto): array
    {
        $doctor = $this->findDoctor($dto->getDoctorId());
        $doctorConfig = $doctor->getDoctorConfig();

        $date = new DateTime($dto->getDate());
        if ($this->checkWorkday($date, $doctorConfig->getWorkdays()))
            throw new NotFoundException("Doctor does not work on this day");

        return $this->findFreeDoctorHours($date, $doctor);
    }

    /**
     * @throws NotFoundException
     * @throws Exception
     */
    public function create(RequestAppointmentDto $dto, string $userIdentifier): ResponseAppointmentDto
    {
        $doctor = $this->findDoctor($dto->getDoctorId());
        $doctorConfig = $doctor->getDoctorConfig();

        $date = new DateTime($dto->getDate());
        $startTime = new DateTime($dto->getStartTime());
        $endTime = $this->createEndTime($startTime, $doctorConfig->getInterval());

        // check that the doctor sees patients on this day of the week
        if ($this->checkWorkday($date, $doctorConfig->getWorkdays()))
            throw new NotFoundException('Doctor does not work on this day');

        // check that you are available at this time
        if (!$this->isDoctorAvailableOnDay($date, $startTime, $doctor))
            throw new NotFoundException('Doctor is not available at this time');

        $patient = $this->patientRepository->findOneByEmail($userIdentifier);
        if (is_null($patient))
            throw new NotFoundException("Patient doesn't exist");

        $appointment = (new Appointment())
            ->setDate($date)
            ->setStartTime($startTime)
            ->setEndTime($endTime)
            ->setIsCanceled(false)
            ->setIsCompleted(false)
            ->setDoctor($doctor)
            ->setPatient($patient);

        $this->appointmentRepository->save($appointment, true);
        return $this->appointmentResponseDtoTransformer->transformFromObject($appointment);
    }

    /**
     * @throws ValidationException
     * @throws Exception
     */
    public function showForUser(?string $userIdentifier, string $dateString, string $userType): iterable
    {
        if (!$this->checkDatePattern($dateString))
            throw new ValidationException('date must be Y-m-d');

        $appointments = $this->appointmentRepository
            ->findByDateForUser(new DateTime($dateString), $userIdentifier, $userType);
        return $this->appointmentResponseDtoTransformer->transformFromObjects($appointments);
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     */
    public function update(int $appointmentId, string $userIdentifier, string $action): ResponseAppointmentDto
    {
        if ($appointmentId <= 0)
            throw new NotFoundException("The appointment doesn't exist");

        $appointment = $this->appointmentRepository->findOneById($appointmentId);
        if (is_null($appointment) || $appointment->getDoctor()->getUser()->getEmail() !== $userIdentifier)
            throw new NotFoundException("The appointment doesn't exist or the doctor doesn't have access");

        if ($action === 'finalize') {
            if ($appointment->isCompleted())
                throw new AlreadyExistException('The appointment is already over');
            $appointment->setIsCompleted(true);
        } elseif ($action === 'cancel') {
            if ($appointment->isCanceled())
                throw new AlreadyExistException('The appointment is already canceled');
            $appointment->setIsCanceled(true);
        }
        $this->appointmentRepository->save($appointment, true);
        return $this->appointmentResponseDtoTransformer->transformFromObject($appointment);
    }
}