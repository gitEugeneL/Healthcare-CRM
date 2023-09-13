<?php

namespace App\Service;

use App\Dto\Doctor\CreateDoctorDto;
use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\Doctor\UpdateStatusDoctorDto;
use App\Entity\Doctor\Doctor;
use App\Entity\Doctor\Status;
use App\Entity\Auth\Roles;
use App\Entity\Auth\User;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\DoctorRepository;
use App\Repository\UserRepository;
use App\Transformer\Doctor\DoctorResponseDtoTransformer;
use App\Transformer\Paginator\PaginatorResponseTransformer;

class DoctorService
{
    public function __construct(
        private readonly DoctorRepository $doctorRepository,
        private readonly UserRepository $userRepository,
        private readonly DoctorResponseDtoTransformer $doctorResponseDtoTransformer,
        private readonly PaginatorResponseTransformer $paginatorResponseTransformer
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateDoctorDto $dto): ResponseDoctorDto
    {
        if ($this->userRepository->isUserExist($dto->getEmail()))
            throw new AlreadyExistException("User {$dto->getEmail()} already exists");

        $doctor = (new Doctor())
            ->setUser((new User())
                ->setEmail($dto->getEmail())
                ->setPassword(password_hash($dto->getPassword(), PASSWORD_DEFAULT))
                ->setRoles([Roles::ROLE_DOCTOR])
                ->setFirstName($dto->getFirstName())
                ->setLastName($dto->getLastName())
            )
            ->setStatus(Status::ACTIVE);
        $this->doctorRepository->save($doctor, true);
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
    }

    public function show(int $page): array
    {
        $itemPerPage = 10;
        $doctors = $this->doctorRepository->findAllWithPagination($page, $itemPerPage);
        $count = $this->doctorRepository->findAllPaginationCount($itemPerPage);
        return $this->paginatorResponseTransformer
            ->transformToArray($this->doctorResponseDtoTransformer->transformFromObjects($doctors), $page, $count);
    }

    /**
     * @throws NotFoundException
     */
    public function showOne(int $doctorId): ResponseDoctorDto
    {
        $doctor = $this->doctorRepository->findOneById($doctorId);
        if (is_null($doctor))
            throw new NotFoundException("User id:{$doctorId} not found");
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
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
}