<?php

namespace App\Dto\Specialization;

use App\Validator\Constraints\PositiveNumber;
use Symfony\Component\Validator\Constraints as Assert;

class UpdateSpecializationDoctorsDto
{
    #[Assert\NotNull]
    #[Assert\NotBlank]
    #[PositiveNumber]
    private mixed $doctorId;

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

    public function getDoctorId(): mixed
    {
        return $this->doctorId;
    }

    public function setDoctorId(mixed $doctorId): void
    {
        $this->doctorId = $doctorId;
    }
}