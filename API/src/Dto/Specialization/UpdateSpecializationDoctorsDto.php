<?php

namespace App\Dto\Specialization;

use Symfony\Component\Validator\Constraints as Assert;

class UpdateSpecializationDoctorsDto
{
    #[Assert\NotNull]
    #[Assert\NotBlank]
    private int $doctorId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    private string $specializationName;

    public function getSpecializationName(): string
    {
        return $this->specializationName;
    }

    public function setSpecializationName(string $specializationName): void
    {
        $this->specializationName = trim(strtolower($specializationName));
    }

    public function getDoctorId(): string
    {
        return $this->doctorId;
    }

    public function setDoctorId(int $doctorId): void
    {
        $this->doctorId = $doctorId;
    }
}