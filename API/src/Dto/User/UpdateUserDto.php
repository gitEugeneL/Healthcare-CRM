<?php

namespace App\Dto\User;
use Symfony\Component\Validator\Constraints as Assert;

class UpdateUserDto
{
    #[Assert\Length(min: 3, max: 50)]
    private ?string $firstName;

    #[Assert\Length(min: 3, max: 100)]
    private ?string $lastName;

    #[Assert\Length(min: 9, max: 12)]
    #[Assert\Regex(
        pattern: '/^(\+)?\d+$/',
        message: "Phone number should start with + (optional) and contain only digits."
    )]
    private ?string $phone;

    public function getLastName(): ?string
    {
        return $this->lastName ?? null;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = $lastName;
    }

    public function getFirstName(): ?string
    {
        return $this->firstName ?? null;
    }

    public function setFirstName(?string $firstName): void
    {
        $this->firstName = trim(strtolower($firstName));
    }

    public function getPhone(): ?string
    {
        return $this->phone ?? null;
    }

    public function setPhone(?string $phone): void
    {
        $this->phone = trim($phone);
    }
}