<?php

namespace App\Repository;

use App\Entity\Doctor\Doctor;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\ORM\NonUniqueResultException;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<Doctor>
 *
 * @method Doctor|null find($id, $lockMode = null, $lockVersion = null)
 * @method Doctor|null findOneBy(array $criteria, array $orderBy = null)
 * @method Doctor[]    findAll()
 * @method Doctor[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class DoctorRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Doctor::class);
    }

    public function save(Doctor $entity, bool $flush = false): void
    {
        $this->getEntityManager()->persist($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function remove(Doctor $entity, bool $flush = false): void
    {
        $this->getEntityManager()->remove($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function findAllWithPagination(int $page, int $limit): array
    {
        $total = count($this->findAll());
        $doctors =  $this->findBy([], [], $limit, ($page - 1) * $limit);

        return [
            'doctors' => $doctors,
            'totalPages' => ceil($total / $limit)
        ];
    }

    public function findBySpecializationWithPagination(string $specializationName, int $page, int $limit): array
    {
        $qb = $this->createQueryBuilder('d');

        $qb->join('d.specializations', 's')
            ->where('s.name = :specializationName')
            ->setParameter('specializationName', $specializationName);

        $total = $qb->select('COUNT(d.id)')
            ->getQuery()
            ->getSingleScalarResult();

        $doctors = $qb->select('d')
            ->setMaxResults($limit)
            ->setFirstResult(($page - 1) * $limit)
            ->getQuery()
            ->getResult();

        return [
            'doctors' => $doctors,
            'totalPages' => ceil($total / $limit)
        ];
    }

    public function findOneById(int $id): Doctor|null
    {
        return $this->findOneBy(['id' => $id]);
    }
}
