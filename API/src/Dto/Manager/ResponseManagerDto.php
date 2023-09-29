<?php

namespace App\Dto\Manager;

use App\Dto\User\ResponseUserDto;

class ResponseManagerDto extends ResponseUserDto
{
    private string $position;

    public function getPosition(): string
    {
        return $this->position;
    }

    public function setPosition(string $position): static
    {
        $this->position = $position;
        return $this;
    }
}