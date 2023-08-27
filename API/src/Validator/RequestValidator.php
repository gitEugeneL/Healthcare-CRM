<?php

namespace App\Validator;

use Symfony\Component\Validator\Validator\ValidatorInterface;

class RequestValidator
{
    public function __construct(
        private readonly ValidatorInterface $validatorInterface
    ) {}

    public function dtoValidator(object $dto) : array
    {
        $errors = $this->validatorInterface->validate($dto);
        if (count($errors) > 0) {
            $errorMessages = [];
            foreach ($errors as $error) {
                $field = $error->getPropertyPath();
                $message = $error->getMessage();
                $errorMessages[$field] = $message;
            }
            return $errorMessages;
        }
        return [];
    }
}