<?php

namespace App\Dto\Doctor;

use App\Dto\User\UpdateUserDto;
use Symfony\Component\Validator\Constraints as Assert;

class UpdateDoctorDto extends UpdateUserDto
{
    #[Assert\Length(min: 10, max: 250)]
    private ?string $description;

    #[Assert\Length(min: 10, max: 250)]
    private ?string $education;

    public function getDescription(): ?string
    {
        return $this->description ?? null;
    }

    public function setDescription(?string $description): void
    {
        $this->description = trim($description);
    }

    public function getEducation(): ?string
    {
        return $this->education ?? null;
    }

    public function setEducation(?string $education): void
    {
        $this->education = trim($education);
    }
}