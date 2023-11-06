<?php

namespace App\Tests\DtoValidator\MedicalRecord;

use App\Constant\ValidationConstants;
use App\Dto\MedicalRecord\CreateMedicalRecordDto;
use App\Tests\DtoTestCase;

class CreateMedicalRecordDtoTest extends DtoTestCase
{
    public function correctMedicalRecords(): array
    {
        return [
            [
                [
                    'title' => 'Checkup Report',
                    'description' => 'The patient visited for a routine checkup. No significant issues were identified.',
                    'doctorNote' =>  'Patient seems to be in good health, with normal vital signs and no complaints.',
                    'patientEmail' => 'patient@example.com',
                    'specializationId' => 3,
                    'appointmentId' => 456
                ],
            ],
            [
                [
                    'title' => 'Dental Examination',
                    'description' => 'The patient came in for a routine dental checkup and cleaning.',
                    'patientEmail' => 'patizczdfent2@example.com',
                    'specializationId' => '52',
                    'appointmentId' => '4568'
                ]
            ]
        ];
    }

    public function incorrectMedicalRecords(): array
    {
        return [
            [
                [
                    'title' => ['', ValidationConstants::BLANK_VALUE],
                    'description' => ['', ValidationConstants::BLANK_VALUE],
                    'patientEmail' => ['some text', ValidationConstants::INVALID_EMAIL],
                    'specializationId' => ['', ValidationConstants::BLANK_VALUE],
                    'appointmentId' => ['', ValidationConstants::BLANK_VALUE]
                ]
            ],
        ];
    }

    /**
     * @dataProvider correctMedicalRecords
     */
    public function testCreateMedicalRecord_withCorrectMedicalRecords(array $data): void
    {
        $dto = new CreateMedicalRecordDto();
        $dto->setTitle($data['title']);
        $dto->setDescription($data['description']);
        if (isset($data['doctorNote']))
            $dto->setDoctorNote($data['doctorNote']);
        $dto->setPatientEmail($data['patientEmail']);
        $dto->setSpecializationId($data['specializationId']);
        $dto->setAppointmentId($data['appointmentId']);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectMedicalRecords
     */
    public function testCreateMedicalRecord_withInCorrectMedicalRecords(array $data): void
    {
        $dto = new CreateMedicalRecordDto();
        $dto->setTitle($data['title'][0]);
        $dto->setDescription($data['description'][0]);
        if (isset($data['doctorNote']))
            $dto->setDoctorNote($data['doctorNote'][0]);
        $dto->setPatientEmail($data['patientEmail'][0]);
        $dto->setSpecializationId($data['specializationId'][0]);
        $dto->setAppointmentId($data['appointmentId'][0]);

        $violations = $this->validator->validate($dto);
        $result = $this->inspect($violations);

        foreach ($data as $k => $v) {
            if (!empty($v[1]))
                $this->assertSame($result[$k], $v[1]);
            else
                $this->assertArrayNotHasKey($k, $result);
        }
    }
}