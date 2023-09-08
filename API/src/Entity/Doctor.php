<?php

namespace App\Entity;

use App\Entity\Auth\User;
use App\Repository\DoctorRepository;
use Doctrine\Common\Collections\ArrayCollection;
use Doctrine\Common\Collections\Collection;
use Doctrine\DBAL\Types\Types;
use Doctrine\ORM\Mapping as ORM;

#[ORM\Entity(repositoryClass: DoctorRepository::class)]
class Doctor
{
    // todo add profile status ----------------------------------------------------------------
    #[ORM\Id]
    #[ORM\GeneratedValue]
    #[ORM\Column]
    private ?int $id = null;

    #[ORM\Column(length: 50)]
    private ?string $firstName = null;

    #[ORM\Column(length: 100)]
    private ?string $lastName = null;

    #[ORM\Column(length: 12, nullable: true)]
    private ?string $phone = null;

    #[ORM\Column(type: Types::TEXT, nullable: true)]
    private ?string $description = null;

    #[ORM\Column(type: Types::TEXT, nullable: true)]
    private ?string $education = null;

    #[ORM\ManyToMany(targetEntity: Specialization::class, inversedBy: 'doctors')]
    private Collection $specialization;

    #[ORM\OneToMany(mappedBy: 'doctor', targetEntity: Disease::class)]
    private Collection $diseases;

    #[ORM\OneToOne(cascade: ['persist', 'remove'])]
    #[ORM\JoinColumn(nullable: false)]
    private ?User $user = null;

    public function __construct()
    {
        $this->specialization = new ArrayCollection();
        $this->diseases = new ArrayCollection();
    }

    public function getId(): ?int
    {
        return $this->id;
    }

    public function getFirstName(): ?string
    {
        return $this->firstName;
    }

    public function setFirstName(string $firstName): static
    {
        $this->firstName = $firstName;
        return $this;
    }

    public function getLastName(): ?string
    {
        return $this->lastName;
    }

    public function setLastName(string $lastName): static
    {
        $this->lastName = $lastName;
        return $this;
    }

    public function getPhone(): ?string
    {
        return $this->phone;
    }

    public function setPhone(?string $phone): static
    {
        $this->phone = $phone;
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
        return $this->specialization;
    }

    public function addSpecialization(Specialization $specialization): static
    {
        if (!$this->specialization->contains($specialization)) {
            $this->specialization->add($specialization);
        }
        return $this;
    }

    public function removeSpecialization(Specialization $specialization): static
    {
        $this->specialization->removeElement($specialization);
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
            $disease->setDoctor($this);
        }
        return $this;
    }

    public function removeDisease(Disease $disease): static
    {
        if ($this->diseases->removeElement($disease)) {
            if ($disease->getDoctor() === $this) {
                $disease->setDoctor(null);
            }
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
}
