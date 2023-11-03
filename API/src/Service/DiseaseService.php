<?php

namespace App\Service;

use App\Dto\Disease\RequestDiseaseDto;
use App\Dto\Disease\ResponseDiseaseDto;
use App\Entity\Disease;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\DiseaseRepository;
use App\Repository\DoctorRepository;
use App\Transformer\Disease\DiseaseResponseDtoTransformer;
use Doctrine\ORM\NonUniqueResultException;

class DiseaseService
{
    public function __construct(
        private readonly DiseaseRepository $diseaseRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly DiseaseResponseDtoTransformer $diseaseResponseDtoTransformer
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(RequestDiseaseDto $dto): ResponseDiseaseDto
    {
        $diseaseName = $dto->getName();
        if ($this->diseaseRepository->doesDiseaseExistByName($diseaseName))
            throw new AlreadyExistException("Disease {$diseaseName} already exists");

        $disease = (new Disease())->setName($diseaseName);
        $this->diseaseRepository->save($disease, true);
        return $this->diseaseResponseDtoTransformer->transformFromObject($disease);
    }

    /**
     * @throws NotFoundException
     */
    public function delete(int $diseaseId): void
    {
        $disease = $this->diseaseRepository->findOneByIdOrThrow($diseaseId);
        $this->diseaseRepository->remove($disease, true);
    }

    /**
     * @throws NonUniqueResultException
     * @throws AlreadyExistException
     * @throws NotFoundException
     */
    public function addDoctor(string $doctorIdentifier, int $diseaseId): void
    {
        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);
        $disease = $this->diseaseRepository->findOneByIdOrThrow($diseaseId);
        // verify that the doctor doesn't have this disease
        if ($disease->getDoctors()->contains($doctor))
            throw new AlreadyExistException('This Doctor has already been added');

        $disease->addDoctor($doctor);
        $this->diseaseRepository->save($disease, true);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function removeDoctor(string $doctorIdentifier, int $diseaseId): void
    {
        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);
        $disease = $this->diseaseRepository->findOneByIdOrThrow($diseaseId);
        // verify that the doctor has this disease
        if (!$disease->getDoctors()->contains($doctor))
            throw new NotFoundException("Doctor doesn't have this disease");

        $disease->removeDoctor($doctor);
        $this->diseaseRepository->save($disease, true);
    }
}