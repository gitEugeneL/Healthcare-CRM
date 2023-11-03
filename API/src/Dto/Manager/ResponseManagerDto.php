<?php

namespace App\Dto\Manager;

use App\Dto\User\AbstractResponseUserDto;

class ResponseManagerDto extends AbstractResponseUserDto
{
    private ?string $position;

    public function getPosition(): ?string
    {
        return $this->position ?? null;
    }

    public function setPosition(?string $position): void
    {
        $this->position = ucfirst(strtolower(trim($position)));
    }
}