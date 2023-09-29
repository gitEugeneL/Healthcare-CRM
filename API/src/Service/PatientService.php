<?php

namespace App\Service;

use App\Repository\PatientRepository;

class PatientService
{
    public function __construct(
        private readonly PatientRepository $patientRepository
    ) {}

    public function create()
    {

    }
}