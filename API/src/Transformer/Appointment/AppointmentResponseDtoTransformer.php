<?php

namespace App\Transformer\Appointment;

use App\Dto\Appointment\ResponseAppointmentDto;
use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Transformer\AbstractResponseDtoTransformer;

class AppointmentResponseDtoTransformer extends AbstractResponseDtoTransformer
{
    public function transformFromObject(object $appointment): ResponseAppointmentDto
    {
        $patient = $appointment->getPatient();
        $patientUser = $patient->getUser();

        $doctor = $appointment->getDoctor();
        $doctorUser = $doctor->getUser();

        return (new ResponseAppointmentDto())
            ->setDate(($appointment->getDate())->format('Y-m-d'))
            ->setStart(($appointment->getStartTime())->format('H:i'))
            ->setEnd(($appointment->getEndTime())->format('H:i'))
            ->setPatient((new ResponsePatientDto())
                ->setId($patient->getId())
                ->setFirstName($patientUser->getFirstName())
                ->setLastName($patientUser->getLastName())
                ->setEmail($patientUser->getEmail()))
            ->setDoctor((new ResponseDoctorDto())
                ->setId($doctor->getId())
                ->setFirstName($doctorUser->getFirstName())
                ->setLastName($doctorUser->getLastName())
                ->setEmail($doctorUser->getEmail()));
    }
}