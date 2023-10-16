<?php

namespace App\Dto\MedicalRecord;

use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Dto\Specialization\ResponseSpecializationDto;

class ResponseMedicalRecordDto
{
    private int $id;
    private string $title;
    private string $description;
    private ?string $doctorNote;
    private int $appointmentId;
    private ResponsePatientDto $patient;
    private ResponseDoctorDto $doctor;
    private ResponseSpecializationDto $specialization;
    private string $createdAt;
    private ?string $updatedAt;

    public function getId(): int
    {
        return $this->id;
    }

    public function setId(int $id): static
    {
        $this->id = $id;
        return $this;
    }

    public function getTitle(): string
    {
        return $this->title;
    }

    public function setTitle(string $title): static
    {
        $this->title = $title;
        return $this;
    }

    public function getDescription(): string
    {
        return $this->description;
    }

    public function setDescription(string $description): static
    {
        $this->description = $description;
        return $this;
    }

    public function getDoctorNote(): ?string
    {
        return $this->doctorNote;
    }

    public function setDoctorNote(?string $doctorNote): static
    {
        $this->doctorNote = $doctorNote;
        return $this;
    }

    public function getAppointmentId(): int
    {
        return $this->appointmentId;
    }

    public function setAppointmentId(int $appointmentId): static
    {
        $this->appointmentId = $appointmentId;
        return $this;
    }

    public function getPatient(): ResponsePatientDto
    {
        return $this->patient;
    }

    public function setPatient(ResponsePatientDto $patient): static
    {
        $this->patient = $patient;
        return $this;
    }

    public function getDoctor(): ResponseDoctorDto
    {
        return $this->doctor;
    }

    public function setDoctor(ResponseDoctorDto $doctor): static
    {
        $this->doctor = $doctor;
        return $this;
    }

    public function getSpecialization(): ResponseSpecializationDto
    {
        return $this->specialization;
    }

    public function setSpecialization(ResponseSpecializationDto $specialization): static
    {
        $this->specialization = $specialization;
        return $this;
    }

    public function getCreatedAt(): string
    {
        return $this->createdAt;
    }

    public function setCreatedAt(string $createdAt): static
    {
        $this->createdAt = $createdAt;
        return $this;
    }

    public function getUpdatedAt(): ?string
    {
        return $this->updatedAt;
    }

    public function setUpdatedAt(?string $updatedAt): static
    {
        $this->updatedAt = $updatedAt;
        return $this;
    }
}