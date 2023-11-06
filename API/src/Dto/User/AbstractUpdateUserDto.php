<?php

namespace App\Dto\User;
use App\Constant\ValidationConstants;
use Symfony\Component\Validator\Constraints as Assert;

abstract class AbstractUpdateUserDto
{
    #[Assert\Length(min: 3, max: 50)]
    private ?string $firstName;

    #[Assert\Length(min: 3, max: 100)]
    private ?string $lastName;

    #[Assert\Length(min: 9, max: 12)]
    #[Assert\Regex(
        pattern: '/^(\+)?\d+$/',
        message: ValidationConstants::INVALID_PHONE_NUMBER
    )]
    private ?string $phone;

    public function getLastName(): ?string
    {
        return $this->lastName ?? null;
    }

    public function setLastName(?string $lastName): void
    {
        $this->lastName = ucfirst(strtolower(trim($lastName)));
    }

    public function getFirstName(): ?string
    {
        return $this->firstName ?? null;
    }

    public function setFirstName(?string $firstName): void
    {
        $this->firstName = ucfirst(strtolower(trim($firstName)));
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