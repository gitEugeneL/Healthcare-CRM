<?php

namespace App\Repository;

use App\Entity\Patient;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<Patient>
 *
 * @method Patient|null find($id, $lockMode = null, $lockVersion = null)
 * @method Patient|null findOneBy(array $criteria, array $orderBy = null)
 * @method Patient[]    findAll()
 * @method Patient[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class PatientRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Patient::class);
    }

    public function save(Patient $entity, bool $flush = false): void
    {
        $this->getEntityManager()->persist($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function remove(Patient $entity, bool $flush = false): void
    {
        $this->getEntityManager()->remove($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function findOneByEmail(string $email): Patient|null
    {
        return $this->createQueryBuilder('p')
            ->join('p.user', 'u')
            ->where('u.email = :email')
            ->setParameter('email', $email)
            ->getQuery()
            ->getOneOrNullResult();
    }

    public function findAllWithPagination(int $page, int $limit): array
    {
        $total = count($this->findAll());
        $patients = $this->findBy([],[], $limit, ($page - 1) * $limit);

        return [
          'patients' => $patients,
          'totalPages' => ceil($total / $limit)
        ];
    }

    public function findOneById(int $patientId): Patient|null
    {
        return $this->findOneBy(['id' => $patientId]);
    }
}
