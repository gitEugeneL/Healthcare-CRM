<?php

namespace App\Validator\Constraints;

namespace App\Validator\Constraints;

use App\Constant\ValidationConstants;
use App\Validator\PositiveNumberValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class PositiveNumber extends Constraint
{
    public string $message = '"{{ value }}"'.' '.ValidationConstants::INCORRECT_ID;

    public function validatedBy(): string
    {
        return PositiveNumberValidator::class;
    }
}