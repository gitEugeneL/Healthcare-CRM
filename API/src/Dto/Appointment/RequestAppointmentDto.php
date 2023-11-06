<?php

namespace App\Dto\Appointment;

use App\Constant\ValidationConstants;
use App\Validator\Constraints\PositiveNumber;
use Symfony\Component\Validator\Constraints as Assert;
use App\Validator as AcmeAssert;

class RequestAppointmentDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[PositiveNumber]
    private mixed $doctorId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[AcmeAssert\Constraints\DateInTheFuture]
    private string $date;

    #[Assert\Regex(
        pattern: '/^(?:0[7-9]|1[0-6]):(?:00|15|30|45)$/',
        message: ValidationConstants::INVALID_TIME
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
        $this->date = trim($date);
    }

    public function getStartTime(): ?string
    {
        return $this->startTime;
    }

    public function setStartTime(?string $startTime): void
    {
        $this->startTime = trim($startTime);
    }
}