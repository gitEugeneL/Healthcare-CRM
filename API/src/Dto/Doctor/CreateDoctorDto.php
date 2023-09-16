<?php

namespace App\Dto\Doctor;

use Symfony\Component\Validator\Constraints as Assert;
use App\Validator as AcmeAssert;

class CreateDoctorDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Email]
    #[AcmeAssert\Constraints\User\UserNotExist]
    private string $email;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(min: 8)]
    #[Assert\Regex(
        pattern: '/^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}\S*$/',
        message: 'The password must contain at least one number, one letter and one character'
    )]
    private string $password;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 50, maxMessage: 'First name should not exceed {{ limit }} characters')]
    private string $firstName;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 100, maxMessage: 'Last name should not exceed {{ limit }} characters')]
    private string $lastName;

    public function getEmail(): string
    {
        return $this->email;
    }

    public function setEmail(string $email): void
    {
        $this->email = trim(strtolower($email));
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
        $this->firstName = trim(strtolower($firstName));
    }

    public function getLastName(): string
    {
        return $this->lastName;
    }

    public function setLastName(string $lastName): void
    {
        $this->lastName = trim(strtolower($lastName));
    }
}