<?php

namespace App\Dto\Transformer;

interface ResponseDtoTransformInterface
{
    public function transformFromObject(object $object): object;
    public function transformFromObjects(iterable $objects): iterable;
}