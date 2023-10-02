<?php

namespace App\Transformer\Patient;

use App\Dto\Address\ResponseAddressDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Transformer\AbstractResponseDtoTransformer;

class PatientResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $patient): ResponsePatientDto
    {
        $user = $patient->getUser();
        $address = $patient->getAddress();

        return (new ResponsePatientDto())
            ->setId($patient->getId())
            ->setFirstName($user->getFirstName())
            ->setLastName($user->getLastName())
            ->setEmail($user->getEmail())
            ->setPhone($user->getPhone())
            ->setPesel($patient->getPesel())
            ->setDateOfBirth($patient->getDateOfBirth())
            ->setInsurance($patient->getInsurance())
            ->setAddress((new ResponseAddressDto())
                ->setCity($address->getCity())
                ->setStreet($address->getStreet())
                ->setProvince($address->getProvince())
                ->setPostalCode($address->getPostalCode())
                ->setHouse($address->getHouseNumber())
                ->setApartment($address->getApartmentNumber())
            );
    }
}