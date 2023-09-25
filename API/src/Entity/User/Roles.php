<?php

namespace App\Entity\User;

enum Roles
{
    const ROLE_ADMIN = 'ROLE_ADMIN';
    const ROLE_MANAGER = 'ROLE_MANAGER';
    const ROLE_DOCTOR = 'ROLE_DOCTOR';
}
