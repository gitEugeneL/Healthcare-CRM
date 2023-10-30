<?php

namespace App\DataFixtures;

use App\Entity\Address;
use App\Entity\Doctor\Doctor;
use App\Entity\Doctor\Status;
use App\Entity\DoctorConfig;
use App\Entity\Manager;
use App\Entity\Patient;
use App\Entity\User\Roles;
use App\Entity\User\User;
use App\Entity\Disease;
use DateTime;
use Doctrine\Bundle\FixturesBundle\Fixture;
use Doctrine\Persistence\ObjectManager;

class AppTestDbFixtures extends Fixture
{
    public function load(ObjectManager $manager): void
    {
        // *************************************************************************
        // update and push test DB
        // php bin/console --env=test doctrine:fixtures:load
        // *************************************************************************

        // create test admin -------------------------------------------------------
        $seedAdmin = (new User())
            ->setEmail('a@a.com')
            ->setPassword(password_hash('admin!1A', PASSWORD_DEFAULT))
            ->setRoles([Roles::ADMIN])
            ->setFirstName('admin')
            ->setLastName('admin');

        // create test manger -------------------------------------------------------
        $seedManager = (new Manager())
            ->setUser((new User())
                ->setEmail('m@m.com')
                ->setPassword(password_hash('manager!1M', PASSWORD_DEFAULT))
                ->setRoles([Roles::MANAGER])
                ->setFirstName('manager')
                ->setLastName('manager')
            );

        // create test doctor -------------------------------------------------------
        $seedDoctor = (new Doctor())
            ->setUser((new User())
                ->setEmail('d@d.com')
                ->setPassword(password_hash('doctor!1', PASSWORD_DEFAULT))
                ->setRoles([Roles::DOCTOR])
                ->setFirstName('doctor')
                ->setLastName('doctor')
            )
            ->setDoctorConfig((new DoctorConfig())
                ->setStartTime(new DateTime('08:00'))
                ->setEndTime(new DateTime('17:00'))
                ->setInterval('1H')
                ->setWorkdays([1, 2, 3, 4, 5])
            )
            ->setStatus(Status::ACTIVE);

        // create test patient ------------------------------------------------------
        $seedPatient = (new Patient())
            ->setUser((new User())
                ->setEmail('p@p.com')
                ->setPassword(password_hash('patient1!A', PASSWORD_DEFAULT))
                ->setRoles([Roles::PATIENT])
                ->setFirstName('patient')
                ->setLastName('patient')
            )
            ->setAddress(new Address());

        $manager->persist($seedAdmin);
        $manager->persist($seedManager);
        $manager->persist($seedDoctor);
        $manager->persist($seedPatient);
        $manager->flush();
    }
}
