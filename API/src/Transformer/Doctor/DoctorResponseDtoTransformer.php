<?php

namespace App\Transformer\Doctor;

use App\Dto\Response\Doctor\DoctorResponseDto;
use App\Transformer\AbstractResponseDtoTransformer;

class DoctorResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $doctor): DoctorResponseDto
    {
        $dto = new DoctorResponseDto();
        $dto->setId($doctor->getId());
        $dto->setFirstName($doctor->getFirstName());
        $dto->setLastName($doctor->getLastName());
        $dto->setEmail($doctor->getUser()->getEmail());
        $dto->setPhone($doctor->getPhone());
        $dto->setEducation($doctor->getEducation());
        $dto->setDescription($doctor->getDescription());
        $dto->setSpecialization($doctor->getSpecializations()->toArray() ?: null);
        $dto->setDiseases($doctor->getDiseases()->toArray() ?: null);
        return $dto;
    }
}