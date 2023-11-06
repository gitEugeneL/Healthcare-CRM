<?php

namespace App\Dto\DoctorConfig;

use App\Constant\ValidationConstants;
use Symfony\Component\Validator\Constraints as Assert;
use App\Validator as AcmeAssert;

class RequestDoctorConfigDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^(0[7-9]|1[0-6]):00$/',
        message: ValidationConstants::INCORRECT_START_TIME
    )]
    private string $startTime;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^(0[8-9]|1[0-7]):00$/',
        message: ValidationConstants::INCORRECT_END_TIME
    )]
    private string $endTime;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^(1H|15M|30M|45M)$/',
        message: ValidationConstants::INCORRECT_INTERVAL
    )]
    private string $interval;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[AcmeAssert\Constraints\WorkdaysArray]
    private mixed $workdays;

    public function getStartTime(): string
    {
        return $this->startTime;
    }

    public function setStartTime(string $startTime): void
    {
        $this->startTime = trim($startTime);
    }

    public function getEndTime(): string
    {
        return $this->endTime;
    }

    public function setEndTime(string $endTime): void
    {
        $this->endTime = trim($endTime);
    }

    public function getInterval(): string
    {
        return $this->interval;
    }

    public function setInterval(string $interval): void
    {
        $this->interval = trim($interval);
    }

    public function getWorkdays(): mixed
    {
        return $this->workdays;
    }

    public function setWorkdays(mixed $workdays): void
    {
        $this->workdays = $workdays;
    }
}