<?php

namespace App\Transformer\Specialization;

use App\Dto\Specialization\ResponseSpecializationDto;
use App\Transformer\AbstractResponseDtoTransformer;

class SpecializationResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $specialization): ResponseSpecializationDto
    {
        return (new ResponseSpecializationDto())
            ->setId($specialization->getId())
            ->setName($specialization->getName())
            ->setDescription($specialization->getDescription());
    }
}