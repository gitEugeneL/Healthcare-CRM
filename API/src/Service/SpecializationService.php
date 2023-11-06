<?php

namespace App\Service;

use App\Dto\Specialization\CreateSpecializationDto;
use App\Dto\Specialization\IncludeExcludeSpecializationDto;
use App\Dto\Specialization\ResponseSpecializationDto;
use App\Dto\Specialization\UpdateSpecializationDto;
use App\Entity\Specialization;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\DoctorRepository;
use App\Repository\SpecializationRepository;
use App\Transformer\Specialization\SpecializationResponseDtoTransformer;

class SpecializationService
{
    public function __construct(
        private readonly SpecializationRepository $specializationRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly SpecializationResponseDtoTransformer $specializationResponseDtoTransformer,
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateSpecializationDto $dto): ResponseSpecializationDto
    {
        $specializationName = $dto->getName();
        if ($this->specializationRepository->doesSpecializationExistByName($specializationName))
            throw new AlreadyExistException("Specialization {$specializationName} already exists");

        $specialization = (new Specialization())->setName($specializationName);
        $this->specializationRepository->save($specialization, true);
        return $this->specializationResponseDtoTransformer->transformFromObject($specialization);
    }

    public function show(): iterable
    {
        $specializations = $this->specializationRepository->findAll();
        return $this->specializationResponseDtoTransformer->transformFromObjects($specializations);
    }

    /**
     * @throws NotFoundException
     */
    public function update(UpdateSpecializationDto $dto, string $specializationName): ResponseSpecializationDto
    {
        $specialization = $this->specializationRepository->findOneByNameOrThrow($specializationName);
        $specialization->setDescription($dto->getDescription());
        $this->specializationRepository->save($specialization, true);
        return $this->specializationResponseDtoTransformer->transformFromObject($specialization);
    }

    /**
     * @throws NotFoundException
     */
    public function delete(string $specializationName): void
    {
        $specialization = $this->specializationRepository->findOneByNameOrThrow($specializationName);
        $this->specializationRepository->remove($specialization, true);
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     */
    public function includeDoctor(IncludeExcludeSpecializationDto $dto): void
    {
        $specialization = $this->specializationRepository->findOneByNameOrThrow($dto->getSpecializationName());
        $doctor = $this->doctorRepository->findOneByIdOrThrow($dto->getDoctorId());

        if ($specialization->getDoctors()->contains($doctor))
            throw new AlreadyExistException('This Doctor has already been added');

        $specialization->addDoctor($doctor);
        $this->specializationRepository->save($specialization, true);
    }

    /**
     * @throws NotFoundException
     */
    public function excludeDoctor(IncludeExcludeSpecializationDto $dto): void
    {
        $specialization = $this->specializationRepository->findOneByNameOrThrow($dto->getSpecializationName());
        $doctor = $this->doctorRepository->findOneByIdOrThrow($dto->getDoctorId());

        if (!$specialization->getDoctors()->contains($doctor))
            throw new NotFoundException("Doctor doesn't have this speciality");

        $specialization->removeDoctor($doctor);
        $this->specializationRepository->save($specialization, true);
    }
}