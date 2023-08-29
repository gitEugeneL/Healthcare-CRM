<?php

namespace App\DataFixtures;

use App\Entity\Manager;
use App\Entity\Roles;
use App\Entity\User;
use Doctrine\Bundle\FixturesBundle\Fixture;
use Doctrine\Persistence\ObjectManager;

class AppTestDbFixtures extends Fixture
{
    public function load(ObjectManager $manager): void
    {
        // *************************************************************************
        // update test push and update test DB
        // php bin/console --env=test doctrine:fixtures:load
        // *************************************************************************

        // create test admin -------------------------------------------------------
        $seedAdmin = (new User())
            ->setEmail('admin@admin.com')
            ->setPassword(password_hash('admin!1A', PASSWORD_DEFAULT))
            ->setRoles([Roles::ROLE_ADMIN]);

        // create test manager -----------------------------------------------------
        $seedManager = (new Manager())
            ->setFirstName('manager1')
            ->setLastName('manager1')
            ->setPhone('+48000000000')
            ->setUser((new User())
                ->setEmail('manager@manager.com')
                ->setPassword(password_hash('manager!1M', PASSWORD_DEFAULT))
                ->setRoles([Roles::ROLE_MANAGER]));

        $manager->persist($seedAdmin);
        $manager->persist($seedManager);
        $manager->flush();
    }
}
