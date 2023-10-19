<?php

namespace App\Repository;

use App\Entity\Patient;
use App\Exception\NotFoundException;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\ORM\NonUniqueResultException;
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

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    public function findOneByEmailOrThrow(string $email): Patient
    {
        $patient = $this->createQueryBuilder('p')
            ->join('p.user', 'u')
            ->where('u.email = :email')
            ->setParameter('email', $email)
            ->getQuery()
            ->getOneOrNullResult();
        if (is_null($patient))
            throw new NotFoundException("Patient: {$email} doesn't exist");
        return $patient;
    }

    /**
     * @throws NotFoundException
     */
    public function findOneByIdOrThrow(int $patientId): Patient
    {
        $patient = $this->findOneBy(['id' => $patientId]);
        if (is_null($patient))
            throw new NotFoundException("Patient id: {$patientId} doesn't exist");
        return $patient;
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
}
