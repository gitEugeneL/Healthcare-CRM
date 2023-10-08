<?php

namespace App\Repository;

use App\Entity\Appointment;
use DateInterval;
use DateTime;
use Doctrine\Bundle\DoctrineBundle\Repository\ServiceEntityRepository;
use Doctrine\Persistence\ManagerRegistry;
use Exception;

/**
 * @extends ServiceEntityRepository<Appointment>
 *
 * @method Appointment|null find($id, $lockMode = null, $lockVersion = null)
 * @method Appointment|null findOneBy(array $criteria, array $orderBy = null)
 * @method Appointment[]    findAll()
 * @method Appointment[]    findBy(array $criteria, array $orderBy = null, $limit = null, $offset = null)
 */
class AppointmentRepository extends ServiceEntityRepository
{
    public function __construct(ManagerRegistry $registry)
    {
        parent::__construct($registry, Appointment::class);
    }

    /**
     * @throws Exception
     */
    public function findFreeHours(
        int $doctorId, DateTime $date, DateTime $startDateTime, DateTime $endDateTime, string $interval): array
    {
        $visits = $this->createQueryBuilder('appointment')
            ->join('appointment.doctor', 'doctor')
            ->where('doctor.id = :doctorId')
            ->andWhere('appointment.date = :date')
            ->andWhere('appointment.isCanceled = false')
            ->setParameter('doctorId', $doctorId)
            ->setParameter('date', $date->format('Y-m-d'))
            ->getQuery()
            ->getResult();

        $interval = new DateInterval("PT{$interval}");

        $busyTime = [];
        foreach ($visits as $visit) {
            $startTime = $visit->getStartTime();
            $busyTime[] = $startTime->format('H:i');
        }

        $freeTimeSegments = [];
        while ($startDateTime < $endDateTime) {
            $currentTime = $startDateTime->format('H:i');
            if (!in_array($currentTime, $busyTime)) {
                $endTime = clone $startDateTime;
                $freeTimeSegments[] = [
                    'start' => $currentTime,
                    'end' => $endTime->add($interval)->format('H:i')
                ];
            }
            $startDateTime->add($interval);
        }
        return $freeTimeSegments;
    }
}
