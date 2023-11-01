<?php

namespace App\Entity\User;

enum Roles
{
    const ADMIN = 'ROLE_ADMIN';
    const MANAGER = 'ROLE_MANAGER';
    const DOCTOR = 'ROLE_DOCTOR';
    const PATIENT = 'ROLE_PATIENT';
}
