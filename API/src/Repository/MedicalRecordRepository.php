<?php

namespace App\Repository;

use App\Entity\MedicalRecord;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Service\MedicalRecordService;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\ORM\NonUniqueResultException;
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

    public function doesMedicalRecordExist(int $appointmentId): bool
    {
        return !is_null($this->createQueryBuilder('medicalRecord')
            ->join('medicalRecord.appointment', 'appointment')
            ->where('appointment.id = :id')
            ->setParameter('id', $appointmentId)
            ->getQuery()
            ->getOneOrNullResult()
        );
    }

    public function findForDoctorIdWithPagination(int $doctorId, int $patientId, int $page, int $limit): array
    {
        $qb = $this->createQueryBuilder('mr');
        $qb->join('mr.doctor', 'd')
            ->join('mr.patient', 'p')
            ->where('d.id = :doctorId')
            ->andWhere('p.id = :patientId')
            ->setParameter('doctorId', $doctorId)
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

    public function findForPatientIdWithPagination(int $patientId, int $page, int $limit): array
    {
        $qb = $this->createQueryBuilder('mr');
        $qb->join('mr.patient', 'p')
            ->where('p.id = :id')
            ->setParameter('id', $patientId)
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

    /**
     * @throws NotFoundException
     */
    public function findOneByIdOrThrow(int $medicalRecordId): MedicalRecord
    {
        $medicalRecord = $this->findOneBy(['id' => $medicalRecordId]);
        if (is_null($medicalRecord))
            throw new NotFoundException("Medical record id: {$medicalRecordId} doesn't exist");
        return $medicalRecord;
    }

    /**
     * @throws NotFoundException
     * @throws NonUniqueResultException
     */
    public function findOneByIdForPatientOrThrow(int $medicalRecordId, int $patientId): MedicalRecord
    {
        $medicalRecord = $this->createQueryBuilder('mr')
            ->join('mr.patient', 'p')
            ->where('mr.id = :medicalRecordId')
            ->andWhere('p.id = :patientId')
            ->setParameter('medicalRecordId', $medicalRecordId)
            ->setParameter('patientId', $patientId)
            ->getQuery()
            ->getOneOrNullResult();
        if (is_null($medicalRecord))
            throw  new NotFoundException(
                "Medical record id: {$medicalRecordId} doesn't exist or patient id: {$patientId} doesn't have access");
        return $medicalRecord;
    }
}
