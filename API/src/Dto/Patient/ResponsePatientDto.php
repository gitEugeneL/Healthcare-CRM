<?php

namespace App\Dto\Patient;

use App\Dto\Address\ResponseAddressDto;
use App\Dto\User\AbstractResponseUserDto;
use App\Entity\Address;
use DateTimeInterface;

class ResponsePatientDto extends AbstractResponseUserDto
{
    private ?string $pesel;
    private ?DateTimeInterface $dateOfBirth;
    private ?string $insurance;
    private ?ResponseAddressDto $address;

    public function getPesel(): ?string
    {
        return $this->pesel;
    }

    public function setPesel(?string $pesel): static
    {
        $this->pesel = $pesel;
        return $this;
    }

    public function getDateOfBirth(): ?DateTimeInterface
    {
        return $this->dateOfBirth;
    }

    public function setDateOfBirth(?DateTimeInterface $dateOfBirth): static
    {
        $this->dateOfBirth = $dateOfBirth;
        return $this;
    }

    public function getInsurance(): ?string
    {
        return $this->insurance;
    }

    public function setInsurance(?string $insurance): static
    {
        $this->insurance = $insurance;
        return $this;
    }

    public function getAddress(): ?ResponseAddressDto
    {
        return $this->address;
    }

    public function setAddress(?ResponseAddressDto $address): static
    {
        $this->address = $address;
        return $this;
    }
}