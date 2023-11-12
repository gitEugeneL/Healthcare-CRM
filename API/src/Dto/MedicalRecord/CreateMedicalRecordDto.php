<?php

namespace App\Dto\MedicalRecord;

use App\Validator\Constraints\PositiveNumber;
use Symfony\Component\Validator\Constraints as Assert;
use OpenApi\Attributes as OA;

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
    #[PositiveNumber]
    #[OA\Property(type: 'integer')]
    private mixed $specializationId;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[PositiveNumber]
    #[OA\Property(type: 'integer')]
    private mixed $appointmentId;

    public function getTitle(): string
    {
        return $this->title;
    }

    public function setTitle(string $title): void
    {
        $this->title = ucfirst(strtolower(trim($title)));
    }

    public function getDescription(): string
    {
        return $this->description;
    }

    public function setDescription(string $description): void
    {
        $this->description = ucfirst(strtolower(trim($description)));
    }

    public function getDoctorNote(): ?string
    {
        return $this->doctorNote ?? null;
    }

    public function setDoctorNote(?string $doctorNote): void
    {
        $this->doctorNote = ucfirst(strtolower(trim($doctorNote)));
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