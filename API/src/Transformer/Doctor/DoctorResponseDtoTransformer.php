<?php

namespace App\Transformer\Doctor;

use App\Dto\Doctor\ResponseDoctorDto;
use App\Transformer\AbstractResponseDtoTransformer;

class DoctorResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $doctor): ResponseDoctorDto
    {
        $specializations = [];
        foreach ($doctor->getSpecializations() as $specialization) {
            $specializations[] = $specialization->getName();
        }
        $diseases = [];
        foreach ($doctor->getDiseases() as $disease) {
            $diseases[] = $disease->getName();
        }
        $user = $doctor->getUser();

        return (new ResponseDoctorDto())
            ->setId($doctor->getId())
            ->setFirstName($user->getFirstName())
            ->setLastName($user->getLastName())
            ->setEmail($user->getEmail())
            ->setPhone($user->getPhone())
            ->setEducation($doctor->getEducation())
            ->setDescription($doctor->getDescription())
            ->setSpecializations($specializations)
            ->setDiseases($diseases);
    }
}