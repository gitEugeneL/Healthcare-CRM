<?php

namespace App\Service;

use App\Dto\DoctorConfig\RequestDoctorConfigDto;
use App\Dto\DoctorConfig\ResponseDoctorConfigDto;
use App\Exception\NotFoundException;
use App\Repository\DoctorConfigRepository;
use App\Repository\DoctorRepository;
use App\Transformer\DoctorConfig\DoctorConfigResponseDtoTransformer;
use DateTime;
use Doctrine\ORM\NonUniqueResultException;
use Exception;

class DoctorConfigService
{
    public function __construct(
        private readonly DoctorConfigRepository $doctorConfigRepository,
        private readonly DoctorRepository $doctorRepository,
        private readonly DoctorConfigResponseDtoTransformer $configResponseDtoTransformer
    ) {}

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     * @throws Exception
     */
    public function config(string $doctorIdentifier, RequestDoctorConfigDto $dto): ResponseDoctorConfigDto
    {
        $doctor = $this->doctorRepository->findOneByEmailOrThrow($doctorIdentifier);

        $config = $doctor->getDoctorConfig()
            ->setStartTime(new DateTime($dto->getStartTime()))
            ->setEndTime(new DateTime($dto->getEndTime()))
            ->setInterval($dto->getInterval())
            ->setWorkdays((array) $dto->getWorkdays());

        $this->doctorConfigRepository->save($config, true);
        return $this->configResponseDtoTransformer->transformFromObject($doctor);
    }
}