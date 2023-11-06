<?php

namespace App\Tests\DtoValidator\Specialization;

use App\Constant\ValidationConstants;
use App\Dto\Specialization\UpdateSpecializationDto;
use App\Tests\DtoTestCase;

class UpdateSpecializationDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            ['The pediatric ward is designed for the treatment of children and adolescents'],
            ['The surgical ward is where patients receive post-operative care after undergoing surgery']

        ];
    }

    public function incorrectData(): array
    {
        return [
            ['some text', ValidationConstants::SHORT_VALUE_10],
            ['123', ValidationConstants::SHORT_VALUE_10],
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testUpdateSpecializationDto_withCorrectData(string $description): void
    {
        $dto = new UpdateSpecializationDto();
        $dto->setDescription($description);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectData
     */
    public function testUpdateSpecializationDto_withIncorrectData(string $value, string $error): void
    {
        $dto = new UpdateSpecializationDto();
        $dto->setDescription($value);

        $violations = $this->validator->validate($dto);
        $result = $this->inspect($violations);

        $this->assertSame($result['description'], $error);
    }
}