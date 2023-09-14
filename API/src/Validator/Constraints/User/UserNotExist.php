<?php

namespace App\Validator\Constraints\User;

use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class UserNotExist extends Constraint
{
    public string $message = 'User "{{ value }}" already exist';
}