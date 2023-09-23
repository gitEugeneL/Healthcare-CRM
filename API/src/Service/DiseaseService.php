<?php

namespace App\Service;

use App\Dto\Disease\CreateDiseaseDto;
use App\Dto\Disease\ResponseDiseaseDto;
use App\Entity\Disease;
use App\Entity\Doctor\Doctor;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\DiseaseRepository;
use App\Repository\DoctorRepository;
use App\Transformer\Disease\DiseaseResponseDtoTransformer;
use function PHPUnit\Framework\throwException;

class DiseaseService
{
    public function __construct(
        private readonly DiseaseRepository $diseaseRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly DiseaseResponseDtoTransformer $diseaseResponseDtoTransformer
    ) {}

    /**
     * @throws NotFoundException
     */
    private function findDisease(int $diseaseId): Disease
    {
        if ($diseaseId <= 0)
            throw new NotFoundException('disease id must be greater than zero');
        $disease = $this->diseaseRepository->findOneById($diseaseId);
        if (is_null($disease))
            throw new NotFoundException('disease not found');
        return $disease;
    }

    /**
     * @throws NotFoundException
     */
    private function findDoctor(string $email): Doctor
    {
        $doctor = $this->doctorRepository->findOneByEmail($email);
        if (is_null($doctor))
            throw new NotFoundException('Doctor not found');
        return $doctor;
    }

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateDiseaseDto $dto): ResponseDiseaseDto
    {
        $name = $dto->getName();
        $disease = $this->diseaseRepository->findOneByName($name);
        if (!is_null($disease))
            throw new AlreadyExistException("Disease {$name} already exist");

        $disease = (new Disease())->setName($name);
        $this->diseaseRepository->save($disease, true);
        return $this->diseaseResponseDtoTransformer->transformFromObject($disease);
    }

    /**
     * @throws NotFoundException
     */
    public function delete(int $diseaseId): void
    {
        $disease = $this->findDisease($diseaseId);
        $this->diseaseRepository->remove($disease, true);
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     */
    public function addDoctor(string $doctorIdentifier, int $diseaseId): void
    {
        $doctor = $this->findDoctor($doctorIdentifier);
        $disease = $this->findDisease($diseaseId);

        if ($disease->getDoctors()->contains($doctor))
            throw new AlreadyExistException('This Doctor has already been added');

        $disease->addDoctor($doctor);
        $this->diseaseRepository->save($disease, true);
    }

    /**
     * @throws NotFoundException
     */
    public function removeDoctor(string $doctorIdentifier, int $diseaseId): void
    {
        $doctor = $this->findDoctor($doctorIdentifier);
        $disease = $this->findDisease($diseaseId);

        if (!$disease->getDoctors()->contains($doctor))
            throw new NotFoundException("Doctor doesn't have this disease");

        $disease->removeDoctor($doctor);
        $this->diseaseRepository->save($disease, true);
    }
}