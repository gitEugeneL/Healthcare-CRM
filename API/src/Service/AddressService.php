<?php

namespace App\Service;

use App\Dto\Address\UpdateAddressDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Exception\NotFoundException;
use App\Repository\AddressRepository;
use App\Transformer\Patient\PatientResponseDtoTransformer;

class AddressService
{
    public function __construct(
        private readonly AddressRepository $addressRepository,
        private readonly PatientResponseDtoTransformer $patientResponseDtoTransformer,
    ) {}

    /**
     * @throws NotFoundException
     */
    public function update(string $userIdentifier, UpdateAddressDto $dto): ResponsePatientDto
    {
        $address = $this->addressRepository->findOneByPatientEmail($userIdentifier);
        if (is_null($address))
            throw new NotFoundException('This patient does not exist');

        $address->setCity($dto->getCity())
            ->setProvince($dto->getProvince())
            ->setStreet($dto->getStreet())
            ->setPostalCode($dto->getPostalCode())
            ->setHouseNumber($dto->getHouse())
            ->setApartmentNumber($dto->getApartment());
        $this->addressRepository->save($address, true);
        return $this->patientResponseDtoTransformer->transformFromObject($address->getPatient());
    }
}