<?php

namespace App\Repository;

use App\Entity\Office;
use App\Exception\NotFoundException;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<Office>
 *
 * @method Office|null find($id, $lockMode = null, $lockVersion = null)
 * @method Office|null findOneBy(array $criteria, array $orderBy = null)
 * @method Office[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class OfficeRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Office::class);
    }

    public function save(Office $entity, bool $flush = false): void
    {
        $this->getEntityManager()->persist($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function remove(Office $entity, bool $flush = false): void
    {
        $this->getEntityManager()->remove($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function officeExistsByNumber(int $number): bool
    {
        return !is_null($this->findOneBy(['number' => $number]));
    }

    public function findAll(): array
    {
        return $this->createQueryBuilder('o')
            ->orderBy('o.number')
            ->getQuery()
            ->getResult();
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByNumberOrThrow(int $number): Office
    {
        $office = $this->createQueryBuilder('o')
            ->where('o.number = :number')
            ->setParameter('number', $number)
            ->getQuery()
            ->getOneOrNullResult();
        if (is_null($office))
            throw new NotFoundException("Office number: {$number} doesn't exist");
        return $office;
    }
}
