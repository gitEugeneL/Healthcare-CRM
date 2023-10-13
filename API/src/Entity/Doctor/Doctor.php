<?php

namespace App\Entity\Doctor;

use App\Entity\Appointment;
use App\Entity\DoctorConfig;
use App\Entity\MedicalRecord;
use App\Entity\User\User;
use App\Entity\Disease;
use App\Entity\Specialization;
use App\Repository\DoctorRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: DoctorRepository::class)]
class Doctor
{
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column]
    private ?int $id = null;

    #[ORM\Column]
    private ?string $status = null;

    #[ORM\Column(type: Types::TEXT, nullable: true)]
    private ?string $description = null;

    #[ORM\Column(type: Types::TEXT, nullable: true)]
    private ?string $education = null;

    #[ORM\ManyToMany(targetEntity: Specialization::class, inversedBy: 'doctors')]
    private Collection $specializations;

    #[ORM\ManyToMany(targetEntity: Disease::class, inversedBy: 'doctors')]
    private Collection $diseases;

    #[ORM\OneToOne(cascade: ['persist', 'remove'])]
    #[ORM\JoinColumn(nullable: false)]
    private ?User $user = null;

    #[ORM\OneToMany(mappedBy: 'doctor', targetEntity: Appointment::class)]
    private Collection $appointments;

    #[ORM\OneToOne(inversedBy: 'doctor', cascade: ['persist', 'remove'])]
    #[ORM\JoinColumn(nullable: false)]
    private ?DoctorConfig $doctorConfig = null;

    #[ORM\OneToMany(mappedBy: 'doctor', targetEntity: MedicalRecord::class)]
    private Collection $medicalRecords;

    public function __construct()
    {
        $this->specializations = new ArrayCollection();
        $this->diseases = new ArrayCollection();
        $this->appointments = new ArrayCollection();
        $this->medicalRecords = new ArrayCollection();
    }

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getStatus(): ?string
    {
        return $this->status;
    }

    public function setStatus(?string $status): static
    {
        $this->status = $status;
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

    public function getEducation(): ?string
    {
        return $this->education;
    }

    public function setEducation(?string $education): static
    {
        $this->education = $education;
        return $this;
    }

    /**
     * @return Collection<int, Specialization>
     */
    public function getSpecializations(): Collection
    {
        return $this->specializations;
    }

    public function addSpecialization(Specialization $specialization): static
    {
        if (!$this->specializations->contains($specialization)) {
            $this->specializations->add($specialization);
            $specialization->addDoctor($this);
        }
        return $this;
    }

    public function removeSpecialization(Specialization $specialization): static
    {
        if ($this->specializations->removeElement($specialization)) {
            $specialization->removeDoctor($this);
        }
        return $this;
    }

    /**
     * @return Collection<int, Disease>
     */
    public function getDiseases(): Collection
    {
        return $this->diseases;
    }

    public function addDisease(Disease $disease): static
    {
        if (!$this->diseases->contains($disease)) {
            $this->diseases->add($disease);
            $disease->addDoctor($this);
        }
        return $this;
    }

    public function removeDisease(Disease $disease): static
    {
        if ($this->diseases->removeElement($disease)) {
            $disease->removeDoctor($this);
        }
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
            $appointment->setDoctor($this);
        }
        return $this;
    }

    public function removeAppointment(Appointment $appointment): static
    {
        if ($this->appointments->removeElement($appointment)) {
            if ($appointment->getDoctor() === $this)
                $appointment->setDoctor(null);
        }
        return $this;
    }

    public function getDoctorConfig(): ?DoctorConfig
    {
        return $this->doctorConfig;
    }

    public function setDoctorConfig(DoctorConfig $doctorConfig): static
    {
        if ($doctorConfig->getDoctor() !== $this) {
            $doctorConfig->setDoctor($this);
        }

        $this->doctorConfig = $doctorConfig;
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
            $medicalRecord->setDoctor($this);
        }

        return $this;
    }

    public function removeMedicalRecord(MedicalRecord $medicalRecord): static
    {
        if ($this->medicalRecords->removeElement($medicalRecord)) {
            // set the owning side to null (unless already changed)
            if ($medicalRecord->getDoctor() === $this) {
                $medicalRecord->setDoctor(null);
            }
        }

        return $this;
    }
}
