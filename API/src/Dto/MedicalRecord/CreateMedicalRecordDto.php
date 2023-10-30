<?php

namespace App\Dto\MedicalRecord;

use Symfony\Component\Validator\Constraints as Assert;

class CreateMedicalRecordDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 50)]
    private string $title;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 250)]
    private string $description;

    #[Assert\Length(max: 100)]
    private ?string $doctorNote;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Email]
    private  string $patientEmail;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^[1-9]\d*$/',
        message: 'specializationID must be an integer and greater than 0'
    )]
    private mixed $specializationId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^[1-9]\d*$/',
        message: 'appointmentID must be an integer and greater than 0'
    )]
    private mixed $appointmentId;

    public function getTitle(): string
    {
        return $this->title;
    }

    public function setTitle(string $title): void
    {
        $this->title = trim($title);
    }

    public function getDescription(): string
    {
        return $this->description;
    }

    public function setDescription(string $description): void
    {
        $this->description = trim($description);
    }

    public function getDoctorNote(): ?string
    {
        return $this->doctorNote ?? null;
    }

    public function setDoctorNote(?string $doctorNote): void
    {
        $this->doctorNote = trim($doctorNote);
    }

    public function getPatientEmail(): string
    {
        return $this->patientEmail;
    }

    public function setPatientEmail(string $patientEmail): void
    {
        $this->patientEmail = $patientEmail;
    }

    public function getSpecializationId(): mixed
    {
        return $this->specializationId;
    }

    public function setSpecializationId(mixed $specializationId): void
    {
        $this->specializationId = $specializationId;
    }

    public function getAppointmentId(): mixed
    {
        return $this->appointmentId;
    }

    public function setAppointmentId(mixed $appointmentId): void
    {
        $this->appointmentId = $appointmentId;
    }
}