<?php

namespace App\Transformer\Manager;

use App\Dto\Response\Manager\ManagerResponseDto;
use App\Transformer\AbstractResponseDtoTransformer;

class ManagerResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $manager): ManagerResponseDto
    {
        $user = $manager->getUser();
        return (new ManagerResponseDto())
            ->setId($manager->getId())
            ->setPosition($manager->getPosition())
            ->setFirstName($user->getFirstName())
            ->setLastName($user->getLastName())
            ->setEmail($user->getEmail())
            ->setPhone($user->getPhone());
    }
}