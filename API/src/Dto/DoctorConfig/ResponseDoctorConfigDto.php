<?php

namespace App\Dto\DoctorConfig;

use OpenApi\Attributes as OA;

class ResponseDoctorConfigDto
{
    private int $doctorId;
    #[OA\Property(default: '09:00')]
    private string $startTime;
    #[OA\Property(default: '17:00')]
    private string $endTime;
    #[OA\Property(default: '1H or 15M or 30M or 45M')]
    private string $interval;

    #[OA\Property(default: [1, 2, 3, 4, 5])]
    /**
     * @var array<integer>
     */
    private array $workdays;

    public function getDoctorId(): int
    {
        return $this->doctorId;
    }

    public function setDoctorId(int $doctorId): static
    {
        $this->doctorId = $doctorId;
        return $this;
    }

    public function getStartTime(): string
    {
        return $this->startTime;
    }

    public function setStartTime(string $startTime): static
    {
        $this->startTime = $startTime;
        return $this;
    }

    public function getEndTime(): string
    {
        return $this->endTime;
    }

    public function setEndTime(string $endTime): static
    {
        $this->endTime = $endTime;
        return $this;
    }

    public function getInterval(): string
    {
        return $this->interval;
    }

    public function setInterval(string $interval): static
    {
        $this->interval = $interval;
        return $this;
    }

    public function getWorkdays(): array
    {
        return $this->workdays;
    }

    public function setWorkdays(array $workdays): static
    {
        $this->workdays = $workdays;
        return $this;
    }
}