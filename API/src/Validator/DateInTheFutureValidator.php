<?php

namespace App\Validator;

use DateTimeImmutable;
use Exception;
use Symfony\Component\Validator\Constraint;
use Symfony\Component\Validator\ConstraintValidator;

class DateInTheFutureValidator extends ConstraintValidator
{
    /**
     * @throws Exception
     */
    public function validate(mixed $value, Constraint $constraint): void
    {
        if ($value === null || $value === '')
            return;

        if (new DateTimeImmutable($value) <= new DateTimeImmutable()) {
            $this->context->buildViolation($constraint->message)
                ->addViolation();
        }
    }
}