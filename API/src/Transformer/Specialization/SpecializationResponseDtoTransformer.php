<?php

namespace App\Transformer\Specialization;

use App\Dto\Specialization\ResponseSpecializationDto;

class SpecializationResponseDtoTransformer
{
    public function transformFromObject(object $specialization): ResponseSpecializationDto
    {
        return (new ResponseSpecializationDto())
            ->setName($specialization->getName());
    }
}