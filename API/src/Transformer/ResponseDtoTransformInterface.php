<?php

namespace App\Transformer;

interface ResponseDtoTransformInterface
{
    public function transformFromObject(object $object): object;
    public function transformFromObjects(iterable $objects): iterable;
}