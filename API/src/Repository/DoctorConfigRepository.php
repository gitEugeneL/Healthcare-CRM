<?php

namespace App\Repository;

use App\Entity\DoctorConfig;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;

/**
 * @extends ServiceEntityRepository<DoctorConfig>
 *
 * @method DoctorConfig|null find($id, $lockMode = null, $lockVersion = null)
 * @method DoctorConfig|null findOneBy(array $criteria, array $orderBy = null)
 * @method DoctorConfig[]    findAll()
 * @method DoctorConfig[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class DoctorConfigRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, DoctorConfig::class);
    }
}
