<?php

namespace App\Service;

use App\Dto\Appointment\RequestAppointmentDto;
use App\Entity\Doctor\Status;
use App\Exception\NotFoundException;
use App\Repository\AppointmentRepository;
use App\Repository\DoctorRepository;
use DateTime;
use Exception;

class AppointmentService
{
    public function __construct(
        private readonly AppointmentRepository $appointmentRepository,
        private readonly DoctorRepository $doctorRepository
    ) {}

    /**
     * @throws NotFoundException
     * @throws Exception
     */
    public function showFreeHours(RequestAppointmentDto $dto): array
    {
        $doctor = $this->doctorRepository->findOneById($dto->getDoctorId());
        if (is_null($doctor) || $doctor->getStatus() === Status::DISABLED)
            throw new NotFoundException("Doctor does not exist or doctor is inactive");

        $doctorConfig = $doctor->getDoctorConfig();
        $date = new DateTime($dto->getDate());
        $day = $date->format('N');
        if (!in_array($day, $doctorConfig->getWorkdays()))
            throw new NotFoundException("Doctor does not work on this day");

        return $this->appointmentRepository->findFreeHours(
                $doctor->getId(),
                new DateTime($dto->getDate()),
                $doctorConfig->getStartTime(),
                $doctorConfig->getEndTime(),
                $doctorConfig->getInterval()
        );
    }
}