<?php

namespace App\Validator;

use DateTime;
use Exception;
use Symfony\Component\Validator\Constraint;
use Symfony\Component\Validator\ConstraintValidator;

class DateInTheFutureValidator extends ConstraintValidator
{
    private const  MAX_DATE = '+1 month';

    /**
     * @throws Exception
     */
    public function validate(mixed $value, Constraint $constraint): void
    {
        if ($value === null || $value === '')
            return;

        $currentDate = new DateTime();
        $valueDate = new DateTime($value);

        if ($valueDate <= $currentDate || $valueDate >= $currentDate->modify(self::MAX_DATE)) {
            $this->context->buildViolation($constraint->message)
                ->addViolation();
        }
    }
}