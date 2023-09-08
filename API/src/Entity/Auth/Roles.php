<?php

namespace App\Entity\Auth;

enum Roles
{
    const ROLE_ADMIN = 'ROLE_ADMIN';
    const ROLE_MANAGER = 'ROLE_MANAGER';
    const ROLE_DOCTOR = 'ROLE_DOCTOR';
}
