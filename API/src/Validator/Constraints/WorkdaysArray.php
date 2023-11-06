<?php

namespace App\Validator\Constraints;

use App\Constant\ValidationConstants;
use App\Validator\WorkdaysArrayValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class WorkdaysArray extends Constraint
{
    public string $message = ValidationConstants::INCORRECT_WORKDAYS;

    public function validatedBy(): string
    {
        return WorkdaysArrayValidator::class;
    }
}