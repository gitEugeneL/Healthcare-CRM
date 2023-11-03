<?php

namespace App\Dto\Patient;

use App\Dto\User\AbstractUpdateUserDto;
use Symfony\Component\Validator\Constraints as Assert;

class UpdatePatientDto extends AbstractUpdateUserDto
{
    #[Assert\Length(11)]
    #[Assert\Regex(
        pattern: '/^\d+$/',
        message: 'PESEL must be a number'
    )]
    private ?string $pesel;

    #[Assert\Regex(
        pattern: '/^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/',
        message: 'DateOfBirth must be Y-m-d (1999-12-31)'
    )]
    private ?string $dateOfBirth;

    #[Assert\Length(max: 255)]
    private ?string $insurance = null;

    public function getPesel(): ?string
    {
        return $this->pesel ?? null;
    }

    public function setPesel(?string $pesel): void
    {
        $this->pesel = trim($pesel);
    }

    public function getDateOfBirth(): ?string
    {
        return $this->dateOfBirth ?? null;
    }

    public function setDateOfBirth(?string $dateOfBirth): void
    {
        $this->dateOfBirth = trim($dateOfBirth);
    }

    public function getInsurance(): ?string
    {
        return $this->insurance ?? null;
    }

    public function setInsurance(?string $insurance): void
    {
        $this->insurance = ucfirst(strtolower(trim($insurance)));
    }
}