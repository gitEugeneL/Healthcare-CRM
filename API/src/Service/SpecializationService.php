<?php

namespace App\Service;

use App\Dto\Specialization\CreateSpecializationDto;
use App\Dto\Specialization\UpdateSpecializationDoctorsDto;
use App\Dto\Specialization\ResponseSpecializationDto;
use App\Dto\Specialization\UpdateSpecializationDto;
use App\Entity\Doctor\Doctor;
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
     * @throws NotFoundException
     */
    private function findSpecialization(string $specializationName): Specialization
    {
        $specialization = $this->specializationRepository->findOneByName($specializationName);
        if (is_null($specialization))
            throw new NotFoundException('Specialization not found');
        return $specialization;
    }

    /**
     * @throws NotFoundException
     */
    private function findDoctor(int $doctorId): Doctor
    {
        $doctor = $this->doctorRepository->findOneById($doctorId);
        if (is_null($doctor))
            throw new NotFoundException('Doctor not found');
        return $doctor;
    }

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateSpecializationDto $dto): ResponseSpecializationDto
    {
        $name = $dto->getName();
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
    public function update(UpdateSpecializationDto $dto, string $specializationName): ResponseSpecializationDto
    {
        $specialization = $this->findSpecialization($specializationName);
        $specialization->setDescription($dto->getDescription());
        $this->specializationRepository->save($specialization, true);
        return $this->specializationResponseDtoTransformer->transformFromObject($specialization);
    }

    /**
     * @throws NotFoundException
     */
    public function delete(string $specializationName): void
    {
        $specialization = $this->findSpecialization($specializationName);
        $this->specializationRepository->remove($specialization, true);
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     */
    public function includeDoctor(UpdateSpecializationDoctorsDto $dto): void
    {
        $specialization = $this->findSpecialization($dto->getSpecializationName());
        $doctor = $this->findDoctor($dto->getDoctorId());

        if ($specialization->getDoctors()->contains($doctor))
            throw new AlreadyExistException('This Doctor has already been added');

        $specialization->addDoctor($doctor);
        $this->specializationRepository->save($specialization, true);
    }

    /**
     * @throws NotFoundException
     */
    public function excludeDoctor(UpdateSpecializationDoctorsDto $dto): void
    {
        $specialization = $this->findSpecialization($dto->getSpecializationName());
        $doctor = $this->findDoctor($dto->getDoctorId());

        if (!$specialization->getDoctors()->contains($doctor))
            throw new NotFoundException("Doctor doesn't have this speciality");

        $specialization->removeDoctor($doctor);
        $this->specializationRepository->save($specialization, true);
    }
}