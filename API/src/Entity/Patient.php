<?php

namespace App\Entity;

use App\Entity\User\User;
use App\Repository\PatientRepository;
use DateTimeInterface;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: PatientRepository::class)]
class Patient
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column]
    private ?int $id = null;

    #[ORM\Column(length: 11, nullable: true)]
    private ?string $pesel = null;

    #[ORM\Column(type: Types::DATE_MUTABLE, nullable: true)]
    private ?DateTimeInterface $dateOfBirth = null;

    #[ORM\Column(length: 255, nullable: true)]
    private ?string $insurance = null;

    #[ORM\OneToOne(cascade: ['persist', 'remove'])]
    #[ORM\JoinColumn(nullable: false)]
    private ?Address $address = null;

    #[ORM\OneToOne(cascade: ['persist', 'remove'])]
    #[ORM\JoinColumn(nullable: false)]
    private ?User $user = null;

    #[ORM\OneToMany(mappedBy: 'patient', targetEntity: Appointment::class)]
    private Collection $appointments;

    #[ORM\OneToMany(mappedBy: 'patient', targetEntity: MedicalRecord::class)]
    private Collection $medicalRecords;

    public function __construct()
    {
        $this->appointments = new ArrayCollection();
        $this->medicalRecords = new ArrayCollection();
    }


    public function getId(): ?int
    {
        return $this->id;
    }

    public function getPesel(): ?string
    {
        return $this->pesel;
    }

    public function setPesel(?string $pesel): static
    {
        $this->pesel = $pesel;
        return $this;
    }

    public function getDateOfBirth(): ?DateTimeInterface
    {
        return $this->dateOfBirth;
    }

    public function setDateOfBirth(?DateTimeInterface $dateOfBirth): static
    {
        $this->dateOfBirth = $dateOfBirth;
        return $this;
    }

    public function getInsurance(): ?string
    {
        return $this->insurance;
    }

    public function setInsurance(?string $insurance): static
    {
        $this->insurance = $insurance;
        return $this;
    }

    public function getUser(): ?User
    {
        return $this->user;
    }

    public function setUser(User $user): static
    {
        $this->user = $user;
        return $this;
    }

    public function getAddress(): ?Address
    {
        return $this->address;
    }

    public function setAddress(?Address $address): static
    {
        $this->address = $address;
        return $this;
    }

    /**
     * @return Collection<int, Appointment>
     */
    public function getAppointments(): Collection
    {
        return $this->appointments;
    }

    public function addAppointment(Appointment $appointment): static
    {
        if (!$this->appointments->contains($appointment)) {
            $this->appointments->add($appointment);
            $appointment->setPatient($this);
        }
        return $this;
    }

    public function removeAppointment(Appointment $appointment): static
    {
        if ($this->appointments->removeElement($appointment)) {
            if ($appointment->getPatient() === $this)
                $appointment->setPatient(null);
        }
        return $this;
    }

    /**
     * @return Collection<int, MedicalRecord>
     */
    public function getMedicalRecords(): Collection
    {
        return $this->medicalRecords;
    }

    public function addMedicalRecord(MedicalRecord $medicalRecord): static
    {
        if (!$this->medicalRecords->contains($medicalRecord)) {
            $this->medicalRecords->add($medicalRecord);
            $medicalRecord->setPatient($this);
        }

        return $this;
    }

    public function removeMedicalRecord(MedicalRecord $medicalRecord): static
    {
        if ($this->medicalRecords->removeElement($medicalRecord)) {
            // set the owning side to null (unless already changed)
            if ($medicalRecord->getPatient() === $this) {
                $medicalRecord->setPatient(null);
            }
        }

        return $this;
    }
}
