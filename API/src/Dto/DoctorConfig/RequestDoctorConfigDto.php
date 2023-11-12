<?php

namespace App\Dto\DoctorConfig;

use App\Constant\ValidationConstants;
use Symfony\Component\Validator\Constraints as Assert;
use App\Validator as AcmeAssert;
use OpenApi\Attributes as OA;

class RequestDoctorConfigDto
{
    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^(0[7-9]|1[0-6]):00$/',
        message: ValidationConstants::INCORRECT_START_TIME
    )]
    #[OA\Property(default: '09:00')]
    private string $startTime;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^(0[8-9]|1[0-7]):00$/',
        message: ValidationConstants::INCORRECT_END_TIME
    )]
    #[OA\Property(default: '17:00')]
    private string $endTime;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[Assert\Regex(
        pattern: '/^(1H|15M|30M|45M)$/',
        message: ValidationConstants::INCORRECT_INTERVAL
    )]
    #[OA\Property(default: '1H or 15M or 30M or 45M')]
    private string $interval;

    #[Assert\NotBlank]
    #[Assert\NotNull]
    #[AcmeAssert\Constraints\WorkdaysArray]
    #[OA\Property(default: [1, 2, 3, 4, 5])]
    /**
     * @var array<integer>
     */
    private array $workdays;

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