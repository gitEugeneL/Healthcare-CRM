<?php

namespace App\Tests\DtoValidator\Doctor;

use App\Dto\Doctor\UpdateStatusDoctorDto;
use App\Tests\DtoTestCase;

class UpdateStatusDoctorDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            [123, 'ACTIVE'],
            ['123', 'DISABLED']
        ];
    }

    public function incorrectData(): array
    {
        $blankValue = 'This value should not be blank.';
        $incorrectId = 'must be an integer and greater than 0';
        $statusValue = "Available value: 'ACTIVE' or 'DISABLED'";

        return [
            [
                [
                    'doctorId' => ['', $blankValue],
                    'status' => ['active', $statusValue]
                ],
                [
                    'doctorId' => ['-10', $incorrectId],
                    'status' => ['-', $statusValue]
                ]
            ]
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testUpdateStatusDoctorDto_withCorrectData(int|string $doctorId, string $status): void
    {
        $dto = new UpdateStatusDoctorDto();
        $dto->setDoctorId($doctorId);
        $dto->setStatus($status);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectData
     */
    public function testUpdateStatusDoctorDto_withIncorrectData(array $data): void
    {
        $dto = new UpdateStatusDoctorDto();
        $dto->setDoctorId($data['doctorId'][0]);
        $dto->setStatus($data['status'][0]);

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

