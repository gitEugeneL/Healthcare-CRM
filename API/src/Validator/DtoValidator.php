<?php

namespace App\Validator;

use App\Exception\ValidationException;
use Symfony\Component\Validator\Validator\ValidatorInterface;

class DtoValidator
{
    public function __construct(
        private readonly ValidatorInterface $validatorInterface
    ) {}

    /**
     * @throws ValidationException
     */
    public function validate($dto): void
    {
        $errors = $this->validatorInterface->validate($dto);
            if (count($errors) > 0) {
                $errorMessages = [];
                foreach ($errors as $error) {
                    $field = $error->getPropertyPath();
                    $message = $error->getMessage();
                    $errorMessages[$field] = $message;
                }
                throw new ValidationException(json_encode($errorMessages));
            }
    }
}