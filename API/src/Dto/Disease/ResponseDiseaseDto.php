<?php

namespace App\Dto\Disease;

class ResponseDiseaseDto
{
    private int $id;
    private string $name;
    private array $doctors;

    public function getName(): string
    {
        return $this->name;
    }

    public function setName(string $name): static
    {
        $this->name = $name;
        return $this;
    }

    public function getId(): int
    {
        return $this->id;
    }

    public function setId(int $id): static
    {
        $this->id = $id;
        return $this;
    }

    public function getDoctors(): array
    {
        return $this->doctors;
    }

    public function setDoctors(array $doctors): static
    {
        $this->doctors = $doctors;
        return $this;
    }
}