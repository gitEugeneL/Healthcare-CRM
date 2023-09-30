<?php

namespace App\Transformer\Patient;

use App\Dto\Patient\ResponsePatientDto;
use App\Transformer\AbstractResponseDtoTransformer;

class PatientResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $patient): ResponsePatientDto
    {
        $user = $patient->getUser();
        return (new ResponsePatientDto())
            ->setId($patient->getId())
            ->setFirstName($user->getFirstName())
            ->setLastName($user->getLastName())
            ->setEmail($user->getEmail())
            ->setPhone($user->getPhone())
            ->setPesel($patient->getPesel())
            ->setDateOfBirth($patient->getDateOfBirth())
            ->setInsurance($patient->getInsurance())
            ->setAddress($patient->getAddress());
    }
}