<?php

namespace App\Dto\Request\Doctor;

use App\Entity\Status;
use Symfony\Component\Validator\Constraints as Assert;

class UpdateStatusDoctorDto
{
    #[Assert\NotNull]
    #[Assert\NotBlank]
    #[Assert\Positive]
    private int $doctorId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Choice([Status::ACTIVE, Status::DISABLED])]
    private string $status;

    public function getDoctorId(): int
    {
        return $this->doctorId;
    }

    public function setDoctorId($doctorId): void
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