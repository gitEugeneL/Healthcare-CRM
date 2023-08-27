<?php

namespace App\Dto\Transformer\Manager;

use App\Dto\Response\Manager\ManagerResponseDto;
use App\Dto\Transformer\AbstractResponseDtoTransformer;

class ManagerResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $manager): ManagerResponseDto
    {
        $dto = new ManagerResponseDto();
        $dto->setFirstName($manager->getFirstName());
        $dto->setLastName($manager->getLastName());
        $dto->setEmail($manager->getUser()->getEmail());
        $dto->setPhone($manager->getPhone());
        return $dto;
    }
}