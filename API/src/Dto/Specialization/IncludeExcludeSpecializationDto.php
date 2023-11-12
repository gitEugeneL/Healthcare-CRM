<?php

namespace App\Dto\Specialization;

use App\Validator\Constraints\PositiveNumber;
use Symfony\Component\Validator\Constraints as Assert;
use OpenApi\Attributes as OA;


class IncludeExcludeSpecializationDto
{
    #[Assert\NotNull]
    #[Assert\NotBlank]
    #[PositiveNumber]
    #[OA\Property(type: 'integer')]
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
        $this->specializationName = ucfirst(strtolower(trim($specializationName)));
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