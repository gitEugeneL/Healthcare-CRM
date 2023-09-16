<?php

namespace App\Dto\Doctor;

use Symfony\Component\Validator\Constraints as Assert;

class UpdateDoctorDto
{
    #[Assert\Length(min: 3, max: 50)]
    private ?string $firstName;

    #[Assert\Length(min: 3, max: 100)]
    private ?string $lastName;

    #[Assert\Length(min: 10, max: 250)]
    private ?string $description;

    #[Assert\Length(min: 10, max: 250)]
    private ?string $education;

    public function getLastName(): ?string
    {
        return $this->lastName ?? null;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getFirstName(): ?string
    {
        return $this->firstName ?? null;
    }

    public function setFirstName(?string $firstName): void
    {
        $this->firstName = trim(strtolower($firstName));
    }

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