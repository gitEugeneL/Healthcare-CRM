<?php

namespace App\Service;

use App\Dto\Disease\CreateDiseaseDto;
use App\Dto\Disease\ResponseDiseaseDto;
use App\Entity\Disease;
use App\Exception\AlreadyExistException;
use App\Repository\DiseaseRepository;
use App\Transformer\Disease\DiseaseResponseDtoTransformer;

class DiseaseService
{
    public function __construct(
        private readonly DiseaseRepository $diseaseRepository,
        private readonly DiseaseResponseDtoTransformer $diseaseResponseDtoTransformer
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(CreateDiseaseDto $dto): ResponseDiseaseDto
    {
        $name = $dto->getName();
        $disease = $this->diseaseRepository->findOneByName($name);
        if (!is_null($disease))
            throw new AlreadyExistException("Disease {$name} already exist");

        $disease = (new Disease())->setName($name);
        $this->diseaseRepository->save($disease, true);
        return $this->diseaseResponseDtoTransformer->transformFromObject($disease);
    }
}