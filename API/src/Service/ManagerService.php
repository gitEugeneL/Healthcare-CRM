<?php

namespace App\Service;

use App\Dto\Manager\CreateManagerDto;
use App\Dto\Manager\ResponseManagerDto;
use App\Dto\Manager\UpdateManagerDto;
use App\Entity\Manager;
use App\Entity\User\Roles;
use App\Entity\User\User;
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
    public function create(CreateManagerDto $dto): ResponseManagerDto
    {
        if ($this->userRepository->isUserExist($dto->getEmail()))
            throw new AlreadyExistException("User {$dto->getEmail()} already exists");

        $manager = (new Manager())
            ->setPosition("new manager")
            ->setUser((new User())
                ->setEmail($dto->getEmail())
                ->setPassword(password_hash($dto->getPassword(), PASSWORD_DEFAULT))
                ->setRoles([Roles::ROLE_MANAGER])
                ->setFirstName($dto->getFirstName())
                ->setLastName($dto->getLastName())
            );
        $this->managerRepository->save($manager, true);
        return $this->managerResponseDtoTransformer->transformFromObject($manager);
    }

    /**
     * @throws NotFoundException
     */
    public function update(UpdateManagerDto $dto, string $userIdentifier): ResponseManagerDto
    {
        if (!$dto->getFirstName() && !$dto->getLastName() && !$dto->getPhone() && !$dto->getPosition())
            throw new NotFoundException('Nothing to change');

        $manager = $this->managerRepository->findOneByEmail($userIdentifier);
        if (is_null($manager))
            throw new NotFoundException("This manager doesn't exist");

        $user = $manager->getUser();
        if (!is_null($dto->getFirstName()))
            $user->setFirstName($dto->getFirstName());
        if (!is_null($dto->getLastName()))
            $user->setLastName($dto->getLastName());
        if (!is_null($dto->getPhone()))
            $user->setPhone($dto->getPhone());
        if (!is_null($dto->getPosition()))
            $manager->setPosition($dto->getPosition());

        $this->managerRepository->save($manager, true);
        return $this->managerResponseDtoTransformer->transformFromObject($manager);
    }

    /**
     * @throws NotFoundException
     */
    public function info(string $userIdentifier): ResponseManagerDto
    {
        $user = $this->userRepository->findOneByEmail($userIdentifier);
        if (is_null($user))
            throw new NotFoundException("This Manager doesn't exist");

        $manager = $this->managerRepository->findByUser($user);
        return $this->managerResponseDtoTransformer->transformFromObject($manager);
    }
}