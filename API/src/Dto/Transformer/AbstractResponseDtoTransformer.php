<?php

namespace App\Dto\Transformer;

abstract class AbstractResponseDtoTransformer implements ResponseDtoTransformInterface
{
    public function transformFromObjects(iterable $objects): iterable
    {
        $dto = [];
        foreach ($objects as $object) {
            $dto[] = $this->transformFromObject($object);
        }
        return $dto;
    }
}