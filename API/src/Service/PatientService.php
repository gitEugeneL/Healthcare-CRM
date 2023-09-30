<?php

namespace App\Service;

use App\Dto\Patient\CreatePatientDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Dto\Patient\UpdatePatientDto;
use App\Entity\Patient;
use App\Entity\User\Roles;
use App\Entity\User\User;
use App\Exception\NotFoundException;
use App\Repository\PatientRepository;
use App\Transformer\Paginator\PaginatorResponseTransformer;
use App\Transformer\Patient\PatientResponseDtoTransformer;
use DateTime;

class PatientService
{
    const ITEM_PER_PAGE = 10;

    public function __construct(
        private readonly PatientRepository $patientRepository,
        private readonly PatientResponseDtoTransformer $patientResponseDtoTransformer,
        private readonly PaginatorResponseTransformer $paginatorResponseTransformer
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

    /**
     * @throws NotFoundException
     * @throws \Exception
     */
    public function update(UpdatePatientDto $dto, string $userIdentifier): ResponsePatientDto
    {
        if (!$dto->getFirstName() && !$dto->getLastName() && !$dto->getPhone() && !$dto->getPesel()
            && !$dto->getInsurance() && !$dto->getDateOfBirth())
            throw new NotFoundException('Nothing to change');

        $patient = $this->patientRepository->findOneByEmail($userIdentifier);
        if (is_null($patient))
            throw new NotFoundException("This patient doesn't exist");

        $user = $patient->getUser();
        if (!is_null($dto->getFirstName()))
            $user->setFirstName($dto->getFirstName());
        if (!is_null($dto->getLastName()))
            $user->setLastName($dto->getLastName());
        if (!is_null($dto->getPhone()))
            $user->setPhone($dto->getPhone());
        if (!is_null($dto->getPesel()))
            $patient->setPesel($dto->getPesel());
        if (!is_null($dto->getInsurance()))
            $patient->setInsurance($dto->getInsurance());
        if (!is_null($dto->getDateOfBirth()))
            $patient->setDateOfBirth(new DateTime($dto->getDateOfBirth()));

        $this->patientRepository->save($patient, true);
        return $this->patientResponseDtoTransformer->transformFromObject($patient);
    }

    public function show(int $page): array
    {
        $result = $this->patientRepository->findAllWithPagination($page, self::ITEM_PER_PAGE);
        $patients = $result['patients'];
        $totalPages = $result['totalPages'];
        return $this->paginatorResponseTransformer
            ->transformToArray($this->patientResponseDtoTransformer->transformFromObjects($patients), $page, $totalPages);
    }

    /**
     * @throws NotFoundException
     */
    public function showOne(int $patientId): ResponsePatientDto
    {
        if ($patientId <= 0)
            throw new NotFoundException('patient id must be greater than zero');
        $patient = $this->patientRepository->findOneById($patientId);
        if (is_null($patient))
            throw new NotFoundException('patient not found');
        return $this->patientResponseDtoTransformer->transformFromObject($patient);
    }
}