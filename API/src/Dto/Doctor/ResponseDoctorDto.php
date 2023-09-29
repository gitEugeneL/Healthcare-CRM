<?php

namespace App\Dto\Doctor;

use App\Dto\User\ResponseUserDto;

class ResponseDoctorDto extends ResponseUserDto
{
    private ?string $education;
    private ?string $description;
    private ?array $specializations;
    private ?array $diseases;

    public function getEducation(): string
    {
        return $this->education;
    }

    public function setEducation(?string $education): static
    {
        $this->education = $education;
        return $this;
    }

    public function getDescription(): string
    {
        return $this->description;
    }

    public function setDescription(?string $description): static
    {
        $this->description = $description;
        return $this;
    }

    public function getSpecializations(): array
    {
        return $this->specializations;
    }

    public function setSpecializations(?array $specializations): static
    {
        $this->specializations = $specializations;
        return $this;
    }

    public function getDiseases(): array
    {
        return $this->diseases;
    }

    public function setDiseases(?array $diseases): static
    {
        $this->diseases = $diseases;
        return $this;
    }
}