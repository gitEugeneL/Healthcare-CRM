<?php

namespace App\Service;

use App\Dto\Request\Doctor\CreateDoctorDto;
use App\Dto\Response\Doctor\DoctorResponseDto;
use App\Entity\Auth\Roles;
use App\Entity\Auth\Status;
use App\Entity\Auth\User;
use App\Entity\Doctor;
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
    public function create(CreateDoctorDto $dto): DoctorResponseDto
    {
        if ($this->userRepository->isUserExist($dto->getEmail()))
            throw new AlreadyExistException("User {$dto->getEmail()} already exists");

        $doctor = (new Doctor())
            ->setUser((new User())
                ->setEmail($dto->getEmail())
                ->setPassword(password_hash($dto->getPassword(), PASSWORD_DEFAULT))
                ->setRoles([Roles::ROLE_DOCTOR])
                ->setStatus(Status::ACTIVE)
                ->setFirstName($dto->getFirstName())
                ->setLastName($dto->getLastName())
            );
        $this->doctorRepository->save($doctor, true);
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
    }

    public function show(int $page): array
    {
        $itemPerPage = 10;
        $doctors = $this->doctorRepository->findAllWithPagination($page, $itemPerPage);
        $count = $this->doctorRepository->countPages($itemPerPage);
        return $this->paginatorResponseTransformer
            ->transformToArray($this->doctorResponseDtoTransformer->transformFromObjects($doctors), $page, $count);
    }

    /**
     * @throws NotFoundException
     */
    public function showOne(int $doctorId): DoctorResponseDto
    {
        $doctor = $this->doctorRepository->findOneById($doctorId);
        if (is_null($doctor))
            throw new NotFoundException("User id:{$doctorId} not found");
        return $this->doctorResponseDtoTransformer->transformFromObject($doctor);
    }
}