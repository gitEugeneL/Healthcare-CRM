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

        $pattern = '/^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$/';
        if (!preg_match($pattern, $value)) {
            $this->context->buildViolation($constraint->invalidFormatMessage)
                ->addViolation();
            return;
        }

        $currentDate = new DateTime();
        $valueDate = new DateTime($value);

        if ($valueDate <= $currentDate || $valueDate >= $currentDate->modify(self::MAX_DATE)) {
            $this->context->buildViolation($constraint->invalidDateMessage)
                ->addViolation();
        }
    }
}