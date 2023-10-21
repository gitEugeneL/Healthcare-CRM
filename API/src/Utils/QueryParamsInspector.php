<?php

namespace App\Utils;

use App\Exception\ValidationException;

class QueryParamsInspector
{
    /**
     * @throws ValidationException
     */
    public function inspect(int $value): bool
    {
        if ($value <= 0)
            throw new ValidationException('Query value (id) must be greater than zero');
        return true;
    }
}