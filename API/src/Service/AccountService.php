<?php

namespace App\Service;

use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\Manager\ResponseManagerDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Repository\DoctorRepository;
use App\Repository\ManagerRepository;
use App\Repository\PatientRepository;
use App\Transformer\Doctor\DoctorResponseDtoTransformer;
use App\Transformer\Manager\ManagerResponseDtoTransformer;
use App\Transformer\Patient\PatientResponseDtoTransformer;
use Doctrine\ORM\NonUniqueResultException;

class AccountService
{
    public function __construct(
        private readonly ManagerRepository $managerRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly PatientRepository $patientRepository,
        private readonly ManagerResponseDtoTransformer  $managerResponseDtoTransformer,
        private readonly DoctorResponseDtoTransformer  $doctorResponseDtoTransformer,
        private readonly PatientResponseDtoTransformer  $patientResponseDtoTransformer,
    ) {}

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function info(string $userIdentifier, string $userRole): ResponseManagerDto|ResponseDoctorDto|ResponsePatientDto
    {
        switch ($userRole) {
            case Roles::MANAGER:
                $manager = $this->managerRepository->findOneByEmailOrThrow($userIdentifier);
                return $this->managerResponseDtoTransformer->transformFromObject($manager);
            case Roles::DOCTOR:
                $doctor = $this->doctorRepository->findOneByEmailOrThrow($userIdentifier);
                return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
            case Roles::PATIENT:
                $patient = $this->patientRepository->findOneByEmailOrThrow($userIdentifier);
                return $this->patientResponseDtoTransformer->transformFromObject($patient);
            default:
                throw new NotFoundException('Invalid role');
        }
    }
}