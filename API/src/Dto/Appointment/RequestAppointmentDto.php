<?php

namespace App\Dto\Appointment;

use Symfony\Component\Validator\Constraints as Assert;
use App\Validator as AcmeAssert;


class RequestAppointmentDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^[1-9]\d*$/',
        message: 'doctorID must be an integer and greater than 0'
    )]
    private mixed $doctorId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[AcmeAssert\Constraints\DateInTheFuture]
    #[Assert\Regex(
        pattern: '/^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/',
        message: 'date must be Y-m-d (1999-12-31)'
    )]
    private string $date;

    #[Assert\Regex(
        pattern: '/^(?:0[7-9]|1[0-6]):(?:00|15|30|45)$/',
        message: 'incorrect time format (07:00|15|30|45 to 16:00|15|30|45)'
    )]
    private ?string $startTime;

    public function getDoctorId(): mixed
    {
        return $this->doctorId;
    }

    public function setDoctorId(mixed $doctorId): void
    {
        $this->doctorId = $doctorId;
    }

    public function getDate(): string
    {
        return $this->date;
    }

    public function setDate(string $date): void
    {
        $this->date = $date;
    }

    public function getStartTime(): ?string
    {
        return $this->startTime;
    }

    public function setStartTime(?string $startTime): void
    {
        $this->startTime = $startTime;
    }
}