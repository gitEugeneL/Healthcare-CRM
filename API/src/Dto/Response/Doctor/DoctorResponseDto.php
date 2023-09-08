<?php

namespace App\Dto\Response\Doctor;

class DoctorResponseDto
{
    private int $id;
    private string $firstName;
    private string $lastName;
    private string $email;
    private ?string $phone;
    private ?string $education;
    private ?string $description;
    private ?array $specialization;
    private ?array $diseases;

    public function getId(): int
    {
        return $this->id;
    }

    public function setId(int $id): void
    {
        $this->id = $id;
    }

    public function getFirstName(): string
    {
        return $this->firstName;
    }

    public function setFirstName(string $firstName): void
    {
        $this->firstName = $firstName;
    }

    public function getLastName(): string
    {
        return $this->lastName;
    }

    public function setLastName(string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getEmail(): string
    {
        return $this->email;
    }

    public function setEmail(string $email): void
    {
        $this->email = $email;
    }

    public function getPhone(): string
    {
        return $this->phone;
    }

    public function setPhone(?string $phone): void
    {
        $this->phone = $phone;
    }

    public function getEducation(): string
    {
        return $this->education;
    }

    public function setEducation(?string $education): void
    {
        $this->education = $education;
    }

    public function getDescription(): string
    {
        return $this->description;
    }

    public function setDescription(?string $description): void
    {
        $this->description = $description;
    }

    public function getSpecialization(): array
    {
        return $this->specialization;
    }

    public function setSpecialization(?array $specialization): void
    {
        $this->specialization = $specialization;
    }

    public function getDiseases(): array
    {
        return $this->diseases;
    }

    public function setDiseases(?array $diseases): void
    {
        $this->diseases = $diseases;
    }
}