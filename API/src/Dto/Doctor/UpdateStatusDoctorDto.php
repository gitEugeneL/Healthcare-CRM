<?php

namespace App\Dto\Doctor;

use App\Entity\Doctor\Status;
use Symfony\Component\Validator\Constraints as Assert;

class UpdateStatusDoctorDto
{
    #[Assert\NotNull]
    #[Assert\NotBlank]
    #[Assert\Regex(
        pattern: '/^[1-9]\d*$/',
        message: 'number must be an integer and greater than 0'
    )]
    private mixed $doctorId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Choice([Status::ACTIVE, Status::DISABLED])]
    private string $status;

    public function getDoctorId(): mixed
    {
        return $this->doctorId;
    }

    public function setDoctorId(mixed $doctorId): void
    {
        $this->doctorId = $doctorId;
    }

    public function getStatus(): string
    {
        return $this->status;
    }

    public function setStatus(string $status): void
    {
        $this->status = $status;
    }
}