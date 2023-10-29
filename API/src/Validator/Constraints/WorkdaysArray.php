<?php

namespace App\Validator\Constraints;

use App\Validator\WorkdaysArrayValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class WorkdaysArray extends Constraint
{
    public string $message = 'incorrect workdays array. available: [1, 2, 3, 4, 5, 6, 7]';

    public function validatedBy(): string
    {
        return WorkdaysArrayValidator::class;
    }
}