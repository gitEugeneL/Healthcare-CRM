<?php

namespace App\Service;

use App\Dto\Address\UpdateAddressDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Exception\NotFoundException;
use App\Repository\AddressRepository;
use App\Transformer\Patient\PatientResponseDtoTransformer;
use Doctrine\ORM\NonUniqueResultException;

class AddressService
{
    public function __construct(
        private readonly AddressRepository $addressRepository,
        private readonly PatientResponseDtoTransformer $patientResponseDtoTransformer,
    ) {}

    /**
     * @throws NotFoundException
     * @throws NonUniqueResultException
     */
    public function update(string $userIdentifier, UpdateAddressDto $dto): ResponsePatientDto
    {
        $address = $this->addressRepository->findOneByPatientEmailOrThrow($userIdentifier);

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