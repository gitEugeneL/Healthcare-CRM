<?php

namespace App\Dto\Address;

use App\Constant\ValidationConstants;
use Symfony\Component\Validator\Constraints as Assert;

class RequestAddressDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 100)]
    private string $city;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 100)]
    private string $street;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 10)]
    #[Assert\Regex(
        pattern: '/^\d{2}-\d{3}$/',
        message: ValidationConstants::INVALID_POSTAL_CODE
    )]
    private string $postalCode;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 100)]
    private string $province;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Length(max: 5)]
    private string $house;


    #[Assert\Length(max: 5)]
    private ?string $apartment;

    public function getCity(): string
    {
        return $this->city;
    }

    public function setCity(string $city): void
    {
        $this->city = trim($city);
    }

    public function getStreet(): string
    {
        return $this->street;
    }

    public function setStreet(string $street): void
    {
        $this->street = ucfirst(strtolower(trim($street)));;
    }

    public function getPostalCode(): string
    {
        return $this->postalCode;
    }

    public function setPostalCode(string $postalCode): void
    {
        $this->postalCode = trim($postalCode);
    }

    public function getProvince(): string
    {
        return $this->province;
    }

    public function setProvince(string $province): void
    {
        $this->province = ucfirst(strtolower(trim($province)));;
    }

    public function getHouse(): string
    {
        return $this->house;
    }

    public function setHouse(string $house): void
    {
        $this->house = trim($house);
    }

    public function getApartment(): ?string
    {
        return $this->apartment ?? null;
    }

    public function setApartment(?string $apartment): void
    {
        $this->apartment = trim($apartment);
    }
}