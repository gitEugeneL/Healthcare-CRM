<?php

namespace App\Service;

use App\Dto\Patient\CreatePatientDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Entity\Patient;
use App\Entity\User\Roles;
use App\Entity\User\User;
use App\Repository\PatientRepository;
use App\Transformer\Patient\PatientResponseDtoTransformer;

class PatientService
{
    public function __construct(
        private readonly PatientRepository $patientRepository,
        private readonly PatientResponseDtoTransformer $patientResponseDtoTransformer
    ) {}

    public function create(CreatePatientDto $dto): ResponsePatientDto
    {
        $patient = (new Patient())
            ->setUser((new User())
                ->setEmail($dto->getEmail())
                ->setPassword(password_hash($dto->getPassword(), PASSWORD_DEFAULT))
                ->setRoles([Roles::PATIENT])
                ->setFirstName($dto->getFirstName())
                ->setLastName($dto->getLastName())
            );
        $this->patientRepository->save($patient, true);
        return $this->patientResponseDtoTransformer->transformFromObject($patient);
    }
}