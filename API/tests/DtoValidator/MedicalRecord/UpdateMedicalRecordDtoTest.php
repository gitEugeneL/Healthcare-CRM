<?php

namespace App\Tests\DtoValidator\MedicalRecord;

use App\Dto\MedicalRecord\UpdateMedicalRecordDto;
use App\Tests\DtoTestCase;

class UpdateMedicalRecordDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            [
                [
                    'title' => 'Follow-up',
                    'description' => 'Incision is healing well, and there are no signs of infection.'
                ]
            ],
            [
                [
                    'doctorNote' => 'Continued monitoring is advised, and the patient is instructed to take antibiotics.'
                ]
            ]
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testUpdateMedicalRecord_withCorrectData(array $data): void
    {
        $dto = new UpdateMedicalRecordDto();
        if (isset($data['title']))
            $dto->setTitle($data['title']);
        if (isset($data['description']))
            $dto->setTitle($data['description']);
        if (isset($data['doctorNote']))
            $dto->setTitle($data['doctorNote']);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }
}