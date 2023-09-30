<?php

namespace App\Dto\Manager;

use App\Dto\User\AbstractUpdateUserDto;
use Symfony\Component\Validator\Constraints as Assert;

class UpdateManagerDto extends AbstractUpdateUserDto
{
    #[Assert\Length(max: 150)]
    private ?string $position;

    public function getPosition(): ?string
    {
        return $this->position ?? null;
    }

    public function setPosition(?string $position): void
    {
        $this->position = trim($position);
    }
}