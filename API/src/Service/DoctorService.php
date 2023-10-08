<?php

namespace App\Service;

use App\Dto\Doctor\CreateDoctorDto;
use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\Doctor\UpdateDoctorDto;
use App\Dto\Doctor\UpdateStatusDoctorDto;
use App\Entity\Doctor\Doctor;
use App\Entity\Doctor\Status;
use App\Entity\DoctorConfig;
use App\Entity\User\Roles;
use App\Entity\User\User;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\DoctorRepository;
use App\Transformer\Doctor\DoctorResponseDtoTransformer;
use App\Transformer\Paginator\PaginatorResponseTransformer;
use DateTime;

class DoctorService
{
    const ITEM_PER_PAGE = 10;

    public function __construct(
        private readonly DoctorRepository $doctorRepository,
        private readonly DoctorResponseDtoTransformer $doctorResponseDtoTransformer,
        private readonly PaginatorResponseTransformer $paginatorResponseTransformer
    ) {}

    public function create(CreateDoctorDto $dto): ResponseDoctorDto
    {
        $doctor = (new Doctor())
            ->setUser((new User())
                ->setEmail($dto->getEmail())
                ->setPassword(password_hash($dto->getPassword(), PASSWORD_DEFAULT))
                ->setRoles([Roles::DOCTOR])
                ->setFirstName($dto->getFirstName())
                ->setLastName($dto->getLastName())
            )
            ->setDoctorConfig((new DoctorConfig())
                ->setStartTime(new DateTime('08:00'))
                ->setEndTime(new DateTime('17:00'))
                ->setInterval('1H')
                ->setWorkdays([1, 2, 3, 4, 5])
            )
            ->setStatus(Status::ACTIVE);
        $this->doctorRepository->save($doctor, true);
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
    }

    public function show(int $page): array
    {
       $result = $this->doctorRepository->findAllWithPagination($page, self::ITEM_PER_PAGE);
       $doctors = $result['doctors'];
       $totalPages = $result['totalPages'];
        return $this->paginatorResponseTransformer
            ->transformToArray($this->doctorResponseDtoTransformer->transformFromObjects($doctors), $page, $totalPages);
    }

    /**
     * @throws NotFoundException
     */
    public function showOne(int $doctorId): ResponseDoctorDto
    {
        $doctor = $this->doctorRepository->findOneById($doctorId);
        if (is_null($doctor))
            throw new NotFoundException("User id: {$doctorId} not found");
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
    }

    public function showBySpecialization(string $specializationName, int $page): array
    {
        $result = $this->doctorRepository
            ->findBySpecializationWithPagination($specializationName, $page, self::ITEM_PER_PAGE);
        $doctors = $result['doctors'];
        $totalPages = $result['totalPages'];
        return $this->paginatorResponseTransformer
            ->transformToArray($this->doctorResponseDtoTransformer->transformFromObjects($doctors), $page, $totalPages);
    }

    /**
     * @throws NotFoundException
     */
    public function showByDisease(int $diseaseId, int $page): array
    {
        if ($diseaseId <= 0)
            throw new NotFoundException('disease id must be greater than zero');

        $result = $this->doctorRepository->findByDiseaseWithPagination($diseaseId, $page, self::ITEM_PER_PAGE);
        $doctors = $result['doctors'];
        $totalPage = $result['totalPages'];
        return $this->paginatorResponseTransformer
            ->transformToArray($this->doctorResponseDtoTransformer->transformFromObjects($doctors), $page, $totalPage);
    }

    /**
     * @throws AlreadyExistException
     * @throws NotFoundException
     */
    public function updateStatus(UpdateStatusDoctorDto $dto): void
    {
        $doctorId = $dto->getDoctorId();
        $newStatus = $dto->getStatus();
        $doctor = $this->doctorRepository->findOneById($doctorId);
        if (is_null($doctor))
            throw new NotFoundException("Doctor id:{$doctorId} not found");

        $status = $doctor->getStatus();
        if ($status === $newStatus)
            throw new AlreadyExistException('Status has already been updated');

        if ($status === Status::ACTIVE || $status === Status::DISABLED)
            $doctor->setStatus($newStatus);
        else
            throw new NotFoundException("Status: {$status} doesn't exist");
    }

    /**
     * @throws NotFoundException
     */
    public function update(UpdateDoctorDto $dto, string $userIdentifier): ResponseDoctorDto
    {
        if (!$dto->getFirstName() && !$dto->getLastName()
            && !$dto->getDescription() && !$dto->getEducation() &&  !$dto->getPhone())
            throw new NotFoundException('Nothing to change');

        $doctor = $this->doctorRepository->findOneByEmail($userIdentifier);
        if (is_null($doctor))
            throw new NotFoundException("This doctor doesn't exist");

        $user = $doctor->getUser();
        if (!is_null($dto->getFirstName()))
            $user->setFirstName($dto->getFirstName());
        if (!is_null($dto->getLastName()))
            $user->setLastName($dto->getLastName());
        if (!is_null($dto->getPhone()))
            $user->setPhone($dto->getPhone());
        if (!is_null($dto->getDescription()))
            $doctor->setDescription($dto->getDescription());
        if (!is_null($dto->getEducation()))
            $doctor->setEducation($dto->getEducation());

        $this->doctorRepository->save($doctor, true);
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
    }
}