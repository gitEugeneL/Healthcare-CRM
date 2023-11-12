<?php

namespace App\Dto\Office;

use App\Validator\Constraints\PositiveNumber;
use Symfony\Component\Validator\Constraints as Assert;
use OpenApi\Attributes as OA;

class RequestOfficeDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 20)]
    private string $name;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[PositiveNumber]
    #[OA\Property(type: 'integer')]
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