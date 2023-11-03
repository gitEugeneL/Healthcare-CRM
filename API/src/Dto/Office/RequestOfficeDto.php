<?php

namespace App\Dto\Office;

use App\Validator\Constraints\PositiveNumber;
use Symfony\Component\Validator\Constraints as Assert;

class RequestOfficeDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 20)]
    private string $name;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[PositiveNumber]
    private mixed $number;

    public function getName(): string
    {
        return $this->name;
    }

    public function setName(string $name): void
    {
        $this->name = ucfirst(strtolower(trim($name)));
    }

    public function getNumber(): mixed
    {
        return $this->number;
    }

    public function setNumber(mixed $number): void
    {
        $this->number = $number;
    }
}