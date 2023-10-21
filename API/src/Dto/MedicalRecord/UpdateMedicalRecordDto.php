<?php

namespace App\Dto\MedicalRecord;

use Symfony\Component\Validator\Constraints as Assert;

class UpdateMedicalRecordDto
{
    #[Assert\Length(max: 50)]
    private ?string $title;

    #[Assert\Length(max: 250)]
    private ?string $description;

    #[Assert\Length(max: 100)]
    private ?string $doctorNote;

    public function getTitle(): ?string
    {
        return $this->title ?? null;
    }

    public function setTitle(?string $title): static
    {
        $this->title = $title;
        return $this;
    }

    public function getDescription(): ?string
    {
        return $this->description ?? null;
    }

    public function setDescription(?string $description): static
    {
        $this->description = $description;
        return $this;
    }

    public function getDoctorNote(): ?string
    {
        return $this->doctorNote ?? null;
    }

    public function setDoctorNote(?string $doctorNote): static
    {
        $this->doctorNote = $doctorNote;
        return $this;
    }
}