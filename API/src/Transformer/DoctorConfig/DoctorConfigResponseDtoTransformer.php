<?php

namespace App\Transformer\DoctorConfig;

use App\Dto\DoctorConfig\ResponseDoctorConfigDto;
use App\Transformer\AbstractResponseDtoTransformer;

class DoctorConfigResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $doctor): ResponseDoctorConfigDto
    {
        $config = $doctor->getDoctorConfig();
        return (new ResponseDoctorConfigDto())
            ->setDoctorId($doctor->getId())
            ->setStartTime($config->getStartTime()->format('H:i'))
            ->setEndTime($config->getEndTime()->format('H:i'))
            ->setInterval($config->getInterval())
            ->setWorkdays($config->getWorkdays());
    }
}