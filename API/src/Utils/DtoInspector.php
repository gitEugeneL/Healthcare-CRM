<?php

namespace App\Utils;

use App\Exception\ValidationException;
use Symfony\Component\Validator\Validator\ValidatorInterface;

class DtoInspector
{
    public function __construct(
        private readonly ValidatorInterface $validatorInterface
    ) {}

    /**
     * @throws ValidationException
     */
    public function inspect($dto): bool
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
        return true;
    }
}