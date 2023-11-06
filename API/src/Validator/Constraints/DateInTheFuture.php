<?php

namespace App\Validator\Constraints;

use App\Constant\ValidationConstants;
use App\Validator\DateInTheFutureValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class DateInTheFuture extends Constraint
{
    public string $invalidDateMessage = ValidationConstants::INVALID_DATE;
    public string $invalidFormatMessage = ValidationConstants::INVALID_DATE_FORMAT;

    public function validatedBy(): string
    {
        return DateInTheFutureValidator::class;
    }
}