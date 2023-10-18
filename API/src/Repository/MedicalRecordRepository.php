<?php

namespace App\Repository;

use App\Entity\MedicalRecord;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<MedicalRecord>
 *
 * @method MedicalRecord|null find($id, $lockMode = null, $lockVersion = null)
 * @method MedicalRecord|null findOneBy(array $criteria, array $orderBy = null)
 * @method MedicalRecord[]    findAll()
 * @method MedicalRecord[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class MedicalRecordRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, MedicalRecord::class);
    }

    public function save(MedicalRecord $entity, bool $flush = false): void
    {
        $this->getEntityManager()->persist($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function remove(MedicalRecord $entity, bool $flush = false): void
    {
        $this->getEntityManager()->remove($entity);
        if ($flush)
            $this->getEntityManager()->flush();
    }

    public function doesAppointmentExist(int $appointmentId): bool
    {
        return !is_null($this->createQueryBuilder('medicalRecord')
            ->join('medicalRecord.appointment', 'appointment')
            ->where('appointment.id = :id')
            ->setParameter('id', $appointmentId)
            ->getQuery()
            ->getOneOrNullResult()
        );
    }

    public function findByPatientIdWithPagination(int $patientId, int $page, int $limit): array
    {
        $qb = $this->createQueryBuilder('mr');

        $qb->join('mr.patient', 'p')
            ->where('p.id = :patientId')
            ->setParameter('patientId', $patientId)
            ->getQuery()
            ->getResult();

        $total = $qb->select('COUNT(mr.id)')
            ->getQuery()
            ->getSingleScalarResult();

        $medicalRecords = $qb->select('mr')
            ->setMaxResults($limit)
            ->setFirstResult(($page - 1) * $limit)
            ->getQuery()
            ->getResult();

        return [
            'medicalRecords' => $medicalRecords,
            'totalPages' => ceil($total / $limit)
        ];
    }
}
