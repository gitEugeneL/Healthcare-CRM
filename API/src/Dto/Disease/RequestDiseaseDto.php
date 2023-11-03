<?php

namespace App\Dto\Disease;

use Symfony\Component\Validator\Constraints as Assert;

class RequestDiseaseDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 50)]
    private string $name;

    public function getName(): string
    {
        return $this->name;
    }

    public function setName(string $name): void
    {
        $this->name = ucfirst(strtolower(trim($name)));
    }
}