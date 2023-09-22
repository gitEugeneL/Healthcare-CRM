<?php

namespace App\Repository;

use App\Entity\Disease;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<Disease>
 *
 * @method Disease|null find($id, $lockMode = null, $lockVersion = null)
 * @method Disease|null findOneBy(array $criteria, array $orderBy = null)
 * @method Disease[]    findAll()
 * @method Disease[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class DiseaseRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Disease::class);
    }

    public function save(Disease $entity, bool $flush = false): void
    {
        $this->getEntityManager()->persist($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function remove(Disease $entity, bool $flush = false): void
    {
        $this->getEntityManager()->remove($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function findOneByName(string $name): Disease|null
    {
        return $this->findOneBy(['name' => $name]);
    }

    public function findOneById(int $id): Disease|null
    {
        return $this->findOneBy(['id' => $id]);
    }
}
