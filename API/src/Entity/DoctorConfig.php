<?php

namespace App\Entity;

use App\Entity\Doctor\Doctor;
use App\Repository\DoctorConfigRepository;
use DateTime;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: DoctorConfigRepository::class)]
class DoctorConfig
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column]
    private ?int $id = null;

    #[ORM\Column(type: Types::TIME_MUTABLE)]
    private ?DateTime $startTime = null;

    #[ORM\Column(type: Types::TIME_MUTABLE)]
    private ?DateTime $endTime = null;

    #[ORM\Column(length: 10)]
    private ?string $interval = null;

    #[ORM\Column(type: Types::SIMPLE_ARRAY)]
    private array $workdays = [];

    #[ORM\OneToOne(mappedBy: 'doctorConfig', cascade: ['persist', 'remove'])]
    private ?Doctor $doctor = null;

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getStartTime(): ?DateTime
    {
        return $this->startTime;
    }

    public function setStartTime(DateTime $startTime): static
    {
        $this->startTime = $startTime;
        return $this;
    }

    public function getEndTime(): ?DateTime
    {
        return $this->endTime;
    }

    public function setEndTime(DateTime $endTime): static
    {
        $this->endTime = $endTime;
        return $this;
    }

    public function getInterval(): ?string
    {
        return $this->interval;
    }

    public function setInterval(string $interval): static
    {
        $this->interval = $interval;
        return $this;
    }

    public function getWorkdays(): array
    {
        return $this->workdays;
    }

    public function setWorkdays(array $workdays): static
    {
        $this->workdays = $workdays;
        return $this;
    }

    public function getDoctor(): ?Doctor
    {
        return $this->doctor;
    }

    public function setDoctor(Doctor $Doctor): static
    {
        $this->doctor = $Doctor;
        return $this;
    }
}
