<?php

namespace App\Dto\Specialization;

use Symfony\Component\Validator\Constraints as Assert;

class CreateSpecializationDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(min: 5, max: 100)]
    private string $name;

    public function getName(): string
    {
        return $this->name;
    }

    public function setName(string $name): void
    {
        $this->name = trim($name);
    }
}