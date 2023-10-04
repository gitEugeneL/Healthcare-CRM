<?php

namespace App\Validator\Constraints;

use App\Validator\UserNotExistValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class UserNotExist extends Constraint
{
    public string $message = 'User "{{ value }}" already exists';

    public function validatedBy(): string
    {
        return UserNotExistValidator::class;
    }
}