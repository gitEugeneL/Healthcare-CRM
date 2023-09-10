<?php

namespace App\Transformer\Manager;

use App\Dto\Manager\ResponseManagerDto;
use App\Transformer\AbstractResponseDtoTransformer;

class ManagerResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $manager): ResponseManagerDto
    {
        $user = $manager->getUser();
        return (new ResponseManagerDto())
            ->setId($manager->getId())
            ->setPosition($manager->getPosition())
            ->setFirstName($user->getFirstName())
            ->setLastName($user->getLastName())
            ->setEmail($user->getEmail())
            ->setPhone($user->getPhone());
    }
}