<?php

namespace App\Dto\DoctorConfig;

class ResponseDoctorConfigDto
{
    private int $doctorId;
    private string $startTime;
    private string $endTime;
    private string $interval;
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