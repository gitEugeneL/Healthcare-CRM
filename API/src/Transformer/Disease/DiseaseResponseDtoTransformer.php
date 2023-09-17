<?php

namespace App\Transformer\Disease;

use App\Dto\Disease\ResponseDiseaseDto;
use App\Transformer\AbstractResponseDtoTransformer;

class DiseaseResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $disease): ResponseDiseaseDto
    {
        $doctors = [];
        foreach ($disease->getDoctors() as $doctor) {
            $user = $doctor->getUser();
            $doctors[] = [
                'firstName' => $user->getFirstName(),
                'lastName' => $user->getLastName(),
                'email' => $user->getEmail()
            ];
        }
        return (new ResponseDiseaseDto())
            ->setId($disease->getId())
            ->setName($disease->getName())
            ->setDoctors($doctors);
    }
}