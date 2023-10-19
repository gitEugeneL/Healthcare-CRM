<?php

namespace App\Repository;

use App\Entity\Specialization;
use App\Exception\NotFoundException;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<Specialization>
 *
 * @method Specialization|null find($id, $lockMode = null, $lockVersion = null)
 * @method Specialization|null findOneBy(array $criteria, array $orderBy = null)
 * @method Specialization[]    findAll()
 * @method Specialization[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class SpecializationRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Specialization::class);
    }

    public function save(Specialization $entity, bool $flush = false): void
    {
        $this->getEntityManager()->persist($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function remove(Specialization $entity, bool $flush = false): void
    {
        $this->getEntityManager()->remove($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function doesSpecializationExistByName(string $name): bool
    {
        return !is_null($this->findOneBy(['name' => $name]));
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByNameOrThrow(string $name): Specialization
    {
        $specialization = $this->findOneBy(['name' => $name]);
        if (is_null($specialization))
            throw new NotFoundException("Specialization: {$name} doesn't exist");
        return $specialization;
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByIdOrThrow(int $id): Specialization
    {
        $specialization = $this->findOneBy(['id' => $id]);
        if (is_null($specialization))
            throw new NotFoundException("Specialization id: {$id} doesn't exist");
        return $specialization;
    }
}
