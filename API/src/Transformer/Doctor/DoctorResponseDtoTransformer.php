<?php

namespace App\Transformer\Doctor;

use App\Dto\Response\Doctor\DoctorResponseDto;
use App\Transformer\AbstractResponseDtoTransformer;

class DoctorResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $doctor): DoctorResponseDto
    {
        $user = $doctor->getUser();
        return (new DoctorResponseDto())
            ->setId($doctor->getId())
            ->setFirstName($user->getFirstName())
            ->setLastName($user->getLastName())
            ->setEmail($user->getEmail())
            ->setPhone($user->getPhone())
            ->setEducation($doctor->getEducation())
            ->setDescription($doctor->getDescription())
            ->setSpecialization($doctor->getSpecializations()->toArray() ?: null)
            ->setDiseases($doctor->getDiseases()->toArray() ?: null);
    }
}