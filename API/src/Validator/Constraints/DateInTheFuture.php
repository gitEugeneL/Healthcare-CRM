<?php

namespace App\Validator\Constraints;

use App\Validator\DateInTheFutureValidator;
use Attribute;
use Symfony\Component\Validator\Constraint;

#[Attribute]
class DateInTheFuture extends Constraint
{
    public string $message = 'Invalid date. Please select a future date or date should not be later than +1 month.';

    public function validatedBy(): string
    {
        return DateInTheFutureValidator::class;
    }
}