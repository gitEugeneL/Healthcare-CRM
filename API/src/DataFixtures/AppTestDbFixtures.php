<?php

namespace App\DataFixtures;

use App\Entity\Auth\Roles;
use App\Entity\Auth\User;
use App\Entity\Status;
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
            ->setEmail('admin@admin.com')
            ->setPassword(password_hash('admin!1A', PASSWORD_DEFAULT))
            ->setRoles([Roles::ROLE_ADMIN])
            ->setFirstName('admin')
            ->setLastName('admin');

        $manager->persist($seedAdmin);
        $manager->flush();
    }
}
