<?php

namespace App\Service;

use App\Dto\Request\Manager\CreateManagerDto;
use App\Dto\Response\Manager\ManagerResponseDto;
use App\Entity\Auth\Roles;
use App\Entity\Auth\Status;
use App\Entity\Auth\User;
use App\Entity\Manager;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\ManagerRepository;
use App\Repository\UserRepository;
use App\Transformer\Manager\ManagerResponseDtoTransformer;

class ManagerService
{
    public function __construct(
        private readonly ManagerRepository $managerRepository,
        private readonly UserRepository $userRepository,
        private readonly ManagerResponseDtoTransformer  $managerResponseDtoTransformer
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateManagerDto $dto): ManagerResponseDto
    {
        if ($this->userRepository->isUserExist($dto->getEmail()))
            throw new AlreadyExistException("User {$dto->getEmail()} already exists");

        $manager = (new Manager())
            ->setPosition("new manager")
            ->setUser((new User())
                ->setEmail($dto->getEmail())
                ->setPassword(password_hash($dto->getPassword(), PASSWORD_DEFAULT))
                ->setRoles([Roles::ROLE_MANAGER])
                ->setStatus(Status::ACTIVE)
                ->setFirstName($dto->getFirstName())
                ->setLastName($dto->getLastName())
            );
        $this->managerRepository->save($manager, true);
        return $this->managerResponseDtoTransformer->transformFromObject($manager);
    }

    /**
     * @throws NotFoundException
     */
    public function info(string $userIdentifier): ManagerResponseDto
    {
        $user = $this->userRepository->findOneByEmail($userIdentifier);
        if (is_null($user))
            throw new NotFoundException("This Manager doesn't exist");

        $manager = $this->managerRepository->findByUser($user);
        return $this->managerResponseDtoTransformer->transformFromObject($manager);
    }
}