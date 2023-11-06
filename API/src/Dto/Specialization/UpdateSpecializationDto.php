<?php

namespace App\Dto\Specialization;

use Symfony\Component\Validator\Constraints as Assert;

class UpdateSpecializationDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(min: 10, max: 250)]
    private string $description;

    public function getDescription(): string
    {
        return $this->description;
    }

    public function setDescription(string $description): void
    {
        $this->description = ucfirst(strtolower(trim($description)));
    }
}