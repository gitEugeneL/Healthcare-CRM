<?php

namespace App\Dto\User;

use App\Constant\ValidationConstants;
use App\Validator as AcmeAssert;
use Symfony\Component\Validator\Constraints as Assert;

abstract class AbstractCreateUserDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Email]
    #[AcmeAssert\Constraints\UserNotExist]
    private string $email;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(min: 8)]
    #[Assert\Regex(
        pattern: '/^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$/',
        message: ValidationConstants::INVALID_PASSWORD
    )]
    private string $password;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 50)]
    private string $firstName;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 100)]
    private string $lastName;

    public function getEmail(): string
    {
        return $this->email;
    }

    public function setEmail(string $email): void
    {
        $this->email = trim($email);
    }

    public function getPassword(): string
    {
        return $this->password;
    }

    public function setPassword(string $password): void
    {
        $this->password = trim($password);
    }

    public function getFirstName(): string
    {
        return $this->firstName;
    }

    public function setFirstName(string $firstName): void
    {
        $this->firstName = ucfirst(strtolower(trim($firstName)));
    }

    public function getLastName(): string
    {
        return $this->lastName;
    }

    public function setLastName(string $lastName): void
    {
        $this->lastName = ucfirst(strtolower(trim($lastName)));
    }
}