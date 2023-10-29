<?php

namespace App\Validator;

use Symfony\Component\Validator\Constraint;
use Symfony\Component\Validator\ConstraintValidator;

class WorkdaysArrayValidator extends ConstraintValidator
{
    public function validate(mixed $value, Constraint $constraint): void
    {
        if ($value === null || $value === '')
            return;

        if (!is_array($value))
            $this->context->buildViolation($constraint->message)->addViolation();

        foreach ($value as $day)
            if ($day < 1 || $day > 7)
                $this->context->buildViolation($constraint->message)->addViolation();
    }

}