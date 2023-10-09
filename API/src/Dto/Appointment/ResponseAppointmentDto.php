<?php

namespace App\Dto\Appointment;

use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\Patient\ResponsePatientDto;

class ResponseAppointmentDto
{
    private string $date;
    private string $start;
    private string $end;
    private ResponsePatientDto $patient;
    private ResponseDoctorDto $doctor;

    public function getDate(): string
    {
        return $this->date;
    }

    public function setDate(string $date): static
    {
        $this->date = $date;
        return $this;
    }

    public function getStart(): string
    {
        return $this->start;
    }

    public function setStart(string $start): static
    {
        $this->start = $start;
        return $this;
    }

    public function getEnd(): string
    {
        return $this->end;
    }

    public function setEnd(string $end): static
    {
        $this->end = $end;
        return $this;
    }

    public function getPatient(): ResponsePatientDto
    {
        return $this->patient;
    }

    public function setPatient(ResponsePatientDto $patient): static
    {
        $this->patient = $patient;
        return $this;
    }

    public function getDoctor(): ResponseDoctorDto
    {
        return $this->doctor;
    }

    public function setDoctor(ResponseDoctorDto $doctor): static
    {
        $this->doctor = $doctor;
        return $this;
    }
}