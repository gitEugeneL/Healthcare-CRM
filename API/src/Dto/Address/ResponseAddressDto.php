<?php

namespace App\Dto\Address;

class ResponseAddressDto
{
    private ?string $city;
    private ?string $province;
    private ?string $postalCode;
    private ?string $street;
    private ?string $house;
    private ?string $apartment;

    public function getCity(): ?string
    {
        return $this->city;
    }

    public function setCity(?string $city): static
    {
        $this->city = $city;
        return $this;
    }

    public function getProvince(): ?string
    {
        return $this->province;
    }

    public function setProvince(?string $province): static
    {
        $this->province = $province;
        return $this;
    }

    public function getPostalCode(): ?string
    {
        return $this->postalCode;
    }

    public function setPostalCode(?string $postalCode): static
    {
        $this->postalCode = $postalCode;
        return $this;
    }

    public function getStreet(): ?string
    {
        return $this->street;
    }

    public function setStreet(?string $street): static
    {
        $this->street = $street;
        return $this;
    }

    public function getHouse(): ?string
    {
        return $this->house;
    }

    public function setHouse(?string $house): static
    {
        $this->house = $house;
        return $this;
    }

    public function getApartment(): ?string
    {
        return $this->apartment;
    }

    public function setApartment(?string $apartment): static
    {
        $this->apartment = $apartment;
        return $this;
    }
}