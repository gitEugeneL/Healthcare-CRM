<?php

namespace App\Transformer\Disease;

use App\Dto\Disease\ResponseDiseaseDto;
use App\Dto\Doctor\ResponseDoctorDto;
use App\Transformer\AbstractResponseDtoTransformer;

class DiseaseResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $disease): ResponseDiseaseDto
    {
        $doctors = [];
        foreach ($disease->getDoctors() as $doctor) {
            $user = $doctor->getUser();
            $doctors[] = (new ResponseDoctorDto())
                ->setFirstName($user->getFirstName())
                ->setLastName($user->getLastName())
                ->setEmail($user->getEmail())
                ->setPhone($user->getPhone())
                ->setEducation($doctor->getEducation())
                ->setDescription($doctor->getDescription());
        }
        return (new ResponseDiseaseDto())
            ->setId($disease->getId())
            ->setName($disease->getName())
            ->setDoctors($doctors);
    }
}