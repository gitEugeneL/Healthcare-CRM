<?php

namespace App\Repository;

use App\Entity\Disease;
use App\Exception\NotFoundException;
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

    public function doesDiseaseExistByName(string $name): bool
    {
        return !is_null($this->findOneBy(['name' => $name]));
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByNameOrThrow(string $name): Disease
    {
        $disease = $this->findOneBy(['name' => $name]);
        if (is_null($disease))
            throw new NotFoundException("Disease: {$name} doesn't exist");
        return $disease;
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByIdOrThrow(int $id): Disease
    {
        $disease = $this->findOneBy(['id' => $id]);
        if (is_null($disease))
            throw new NotFoundException("Disease id: {$id} doesn't exist");
        return $disease;
    }
}
