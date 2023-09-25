<?php

namespace App\DataFixtures;

use App\Entity\User\Roles;
use App\Entity\User\User;
use App\Entity\Disease;
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


        // create test disease
        $seedDisease = (new Disease())
            ->setName('influenza');

        $manager->persist($seedAdmin);
        $manager->persist($seedDisease);
        $manager->flush();
    }
}
