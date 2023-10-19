<?php

namespace App\Repository;

use App\Entity\User\User;
use App\Exception\NotFoundException;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<User>
 *
 * @method User|null find($id, $lockMode = null, $lockVersion = null)
 * @method User|null findOneBy(array $criteria, array $orderBy = null)
 * @method User[]    findAll()
 * @method User[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class UserRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, User::class);
    }

    public function doesUserExist(string $email): bool
    {
        return !is_null($this->findOneBy(["email" => $email]));
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByEmailOrThrow(string $email): User
    {
        $user = $this->findOneBy(['email' => $email]);
        if (is_null($user))
            throw new NotFoundException("User: {$email} doesn't exist");
        return $user;
    }
}
