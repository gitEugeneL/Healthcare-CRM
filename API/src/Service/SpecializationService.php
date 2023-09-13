<?php

namespace App\Service;

use App\Dto\Specialization\CreateSpecializationDto;
use App\Dto\Specialization\ResponseSpecializationDto;
use App\Entity\Specialization;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\SpecializationRepository;
use App\Transformer\Doctor\DoctorResponseDtoTransformer;
use App\Transformer\Specialization\SpecializationResponseDtoTransformer;

class SpecializationService
{
    public function __construct(
        private readonly SpecializationRepository $specializationRepository,
        private readonly SpecializationResponseDtoTransformer $specializationResponseDtoTransformer,
        private readonly DoctorResponseDtoTransformer $doctorResponseDtoTransformer,
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateSpecializationDto $dto): ResponseSpecializationDto
    {
        $name = strtolower($dto->getName());
        if (!is_null($this->specializationRepository->findOneByName($name)))
            throw new AlreadyExistException("Specialization {$name} already exists");

        $specialization = (new Specialization())->setName($name);
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
    public function showDoctors(string $specializationName): iterable
    {
        $specialization = $this->specializationRepository->findOneByName($specializationName);
        if (is_null($specialization))
            throw new NotFoundException("Specialization {$specializationName} doesn't exist");
        $doctors = $specialization->getDoctors();
        return $this->doctorResponseDtoTransformer->transformFromObjects($doctors);
    }
}