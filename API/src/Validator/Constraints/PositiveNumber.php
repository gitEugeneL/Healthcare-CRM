<?php

namespace App\Validator\Constraints;

namespace App\Validator\Constraints;

use App\Validator\PositiveNumberValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class PositiveNumber extends Constraint
{
    public string $message = '"{{ value }}" must be an integer and greater than 0';

    public function validatedBy(): string
    {
        return PositiveNumberValidator::class;
    }
}