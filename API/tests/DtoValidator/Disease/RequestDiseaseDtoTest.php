<?php

namespace App\Tests\DtoValidator\Disease;

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
        $blankValue = 'This value should not be blank.';
        $longValue = 'This value is too long. It should have 50 characters or less.';

        return [
            ['', $blankValue],
            ['test text test text test text test text test text test text', $longValue]];
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