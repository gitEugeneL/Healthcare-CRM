<?php

namespace App\Tests\DtoValidator\Specialization;

use App\Constant\ValidationConstants;
use App\Dto\Specialization\IncludeExcludeSpecializationDto;
use App\Tests\DtoTestCase;

class IncludeExcludeSpecializationDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            [25, 'Pediatric Ward'],
            ['89', 'Neurologist']
        ];
    }

    public function incorrectData(): array
    {
        return [
            [
                [
                    'doctorId' => ['asd', '"asd"'.' '.ValidationConstants::INCORRECT_ID],
                    'specializationName' => ['', ValidationConstants::BLANK_VALUE]
                ]
            ],
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testIncludeExcludeSpecializationDto_withCorrectData(
        string|int $doctorId, string $specializationName
    ): void
    {
        $dto = new IncludeExcludeSpecializationDto();
        $dto->setDoctorId($doctorId);
        $dto->setSpecializationName($specializationName);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectData
     */
    public function testIncludeExcludeSpecializationDto_withIncorrectData(array $data): void
    {
        $dto = new IncludeExcludeSpecializationDto();
        $dto->setDoctorId($data['doctorId'][0]);
        $dto->setSpecializationName($data['specializationName'][0]);

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