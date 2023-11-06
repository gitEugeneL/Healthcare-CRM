<?php

namespace App\Tests\DtoValidator\Disease;

use App\Constant\ValidationConstants;
use App\Dto\Disease\RequestDiseaseDto;
use App\Tests\DtoTestCase;

class RequestDiseaseDtoTest extends DtoTestCase
{
    public function correctDiseases(): array
    {
        return [
            ['Botulism'],
            ['Coxsackievirus'],
            ['Haemophilus infection']
        ];
    }

    public function incorrectDiseases(): array
    {
        return [
            ['', ValidationConstants::BLANK_VALUE],
            ['test text test text test text test text test text test text', ValidationConstants::LONG_VALUE_50]
        ];
    }

    /**
     * @dataProvider correctDiseases
     */
    public function testRequestDiseaseDto_withCorrectDiseases(string $name): void
    {
        $dto = new RequestDiseaseDto();
        $dto->setName($name);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectDiseases
     */
    public function testRequestDiseaseDto_withIncorrectDiseases(string $value, string $error): void
    {
        $dto = new RequestDiseaseDto();
        $dto->setName($value);

        $violations = $this->validator->validate($dto);
        $result = $this->inspect($violations);

        $this->assertSame($result['name'], $error);
    }
}