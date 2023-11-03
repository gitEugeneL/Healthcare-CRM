<?php

namespace App\Dto\Specialization;

use Symfony\Component\Validator\Constraints as Assert;

class CreateSpecializationDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(min: 5, max: 100)]
    private string $name;

    #[Assert\Length(max: 250)]
    private ?string $description;

    public function getName(): string
    {
        return $this->name;
    }

    public function setName(string $name): void
    {
        $this->name = ucfirst(strtolower(trim($name)));
    }

    public function getDescription(): ?string
    {
        return $this->description;
    }

    public function setDescription(?string $description): void
    {
        $this->description = ucfirst(strtolower(trim($description)));
    }
}