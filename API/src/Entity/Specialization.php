<?php

namespace App\Entity;

use App\Entity\Doctor\Doctor;
use App\Repository\SpecializationRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: SpecializationRepository::class)]
class Specialization
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column]
    private ?int $id = null;

    #[ORM\Column(length: 100)]
    private ?string $name = null;

    #[ORM\Column(type: Types::TEXT, nullable: true)]
    private ?string $description = null;

    #[ORM\ManyToMany(targetEntity: Doctor::class, mappedBy: 'specializations')]
    private Collection $doctors;

    #[ORM\OneToMany(mappedBy: 'specialization', targetEntity: MedicalRecord::class)]
    private Collection $medicalRecords;

    public function __construct()
    {
        $this->doctors = new ArrayCollection();
        $this->medicalRecords = new ArrayCollection();
    }

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(string $name): static
    {
        $this->name = $name;
        return $this;
    }

    public function getDescription(): ?string
    {
        return $this->description;
    }

    public function setDescription(?string $description): static
    {
        $this->description = $description;
        return $this;
    }

    /**
     * @return Collection<int, Doctor>
     */
    public function getDoctors(): Collection
    {
        return $this->doctors;
    }

    public function addDoctor(Doctor $doctor): static
    {
        if (!$this->doctors->contains($doctor)) {
            $this->doctors->add($doctor);
            $doctor->addSpecialization($this);
        }
        return $this;
    }

    public function removeDoctor(Doctor $doctor): static
    {
        if ($this->doctors->removeElement($doctor)) {
            $doctor->removeSpecialization($this);
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
            $medicalRecord->setSpecialization($this);
        }

        return $this;
    }

    public function removeMedicalRecord(MedicalRecord $medicalRecord): static
    {
        if ($this->medicalRecords->removeElement($medicalRecord)) {
            // set the owning side to null (unless already changed)
            if ($medicalRecord->getSpecialization() === $this) {
                $medicalRecord->setSpecialization(null);
            }
        }

        return $this;
    }
}
