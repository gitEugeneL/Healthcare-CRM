<?php

namespace App\Tests;

use PHPUnit\Framework\TestCase;
use Symfony\Component\Validator\Validation;
use Symfony\Component\Validator\Validator\ValidatorInterface;

class DtoTestCase extends TestCase
{




    protected ValidatorInterface $validator;

    protected function setUp(): void
    {
        $this->validator = Validation::createValidatorBuilder()
            ->enableAnnotationMapping()
            ->getValidator();
    }

    protected function inspect($violations): array
    {
        $errorMessages = [];
        if (count($violations) > 0) {
            foreach ($violations as $error) {
                $field = $error->getPropertyPath();
                $message = $error->getMessage();
                $errorMessages[$field] = $message;
            }
        }
        return $errorMessages;
    }
}