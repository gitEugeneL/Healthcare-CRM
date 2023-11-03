<?php

namespace App\Service;

use App\Dto\Office\RequestOfficeDto;
use App\Entity\Office;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Repository\OfficeRepository;

class OfficeService
{
    public function __construct(
        private readonly OfficeRepository $officeRepository
    ) {}

    /**
     * @throws AlreadyExistException
     */
    public function create(RequestOfficeDto $dto): Office
    {
        $number = $dto->getNumber();
        if ($this->officeRepository->officeExistsByNumber($number))
            throw new AlreadyExistException("Office number: {$number} already exists");

        $office = (new Office())
            ->setNumber($dto->getNumber())
            ->setName($dto->getName())
            ->setIsAvailable(false);
        $this->officeRepository->save($office, true);
        return $office;
    }

    public function show(): array
    {
        return $this->officeRepository->findAll();
    }

    /**
     * @throws NotFoundException
     */
    public function update(RequestOfficeDto $dto): Office
    {
        $office = $this->officeRepository->findOneByNumberOrThrow($dto->getNumber());
        $office->setName($dto->getName());
        $this->officeRepository->save($office, true);
        return $office;
    }

    /**
     * @throws NotFoundException
     */
    public function changeStatus(int $number): Office
    {
        $office = $this->officeRepository->findOneByNumberOrThrow($number);
        $office->setIsAvailable(!$office->getIsAvailable());
        $this->officeRepository->save($office, true);
        return $office;
    }

    /**
     * @throws NotFoundException
     */
    public function delete(int $number): void
    {
        $office = $this->officeRepository->findOneByNumberOrThrow($number);
        $this->officeRepository->remove($office, true);
    }
}