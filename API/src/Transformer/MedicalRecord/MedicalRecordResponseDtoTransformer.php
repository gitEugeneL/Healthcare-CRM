<?php

namespace App\Transformer\MedicalRecord;

use App\Dto\Doctor\ResponseDoctorDto;
use App\Dto\MedicalRecord\ResponseMedicalRecordDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Dto\Specialization\ResponseSpecializationDto;
use App\Transformer\AbstractResponseDtoTransformer;

class MedicalRecordResponseDtoTransformer extends AbstractResponseDtoTransformer
{

    public function transformFromObject(object $medicalRecord): ResponseMedicalRecordDto
    {
        $patient = $medicalRecord->getPatient();
        $patientUser = $patient->getUser();
        $doctor = $medicalRecord->getDoctor();
        $doctorUser = $doctor->getUser();
        $specialization = $medicalRecord->getSpecialization();

        return (new ResponseMedicalRecordDto())
            ->setId($medicalRecord->getId())
            ->setTitle($medicalRecord->getTitle())
            ->setDescription($medicalRecord->getDescription())
            ->setDoctorNote($medicalRecord->getDoctorNote())
            ->setAppointmentId($medicalRecord->getAppointment()->getId())
            ->setPatient((new ResponsePatientDto())
                ->setId($patient->getId())
                ->setFirstName($patientUser->getFirstName())
                ->setLastName($patientUser->getFirstName())
                ->setEmail($patientUser->getEmail())
            )
            ->setDoctor((new ResponseDoctorDto())
                ->setId($doctor->getId())
                ->setFirstName($doctorUser->getFirstName())
                ->setLastName($doctorUser->getLastName())
                ->setEmail($doctorUser->getEmail())
            )
            ->setSpecialization((new ResponseSpecializationDto())
                ->setId($specialization->getId())
                ->setName($specialization->getName())
                ->setDescription($specialization->getDescription())
            )
            ->setCreatedAt($medicalRecord->getCreatedAt()->format('Y-m-d H:i'))
            ->setUpdatedAt(
                $medicalRecord->getUpdatedAt() ? $medicalRecord->getUpdatedAt()->format('Y-m-d H:i') : null);
    }
}