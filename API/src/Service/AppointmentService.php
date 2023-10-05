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
    private const START_TIME = '08:00';
    private const END_TIME = '19:00';
    private const INTERVAL = '1H';

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

        // todo get doctor startTime, endTime, interval and days

        return $this->appointmentRepository
            ->findFreeHours($doctor->getId(),
                new DateTime($dto->getDate()), self::START_TIME, self::END_TIME, self::INTERVAL);
    }
}