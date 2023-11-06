<?php

namespace App\Tests\DtoValidator\Specialization;

use App\Constant\ValidationConstants;
use App\Dto\Specialization\CreateSpecializationDto;
use App\Tests\DtoTestCase;

class CreateSpecializationDtoTest extends DtoTestCase
{
    public function correctSpecializations(): array
    {
        return [
            ['Pediatric Ward', 'The pediatric ward is designed for the treatment of children and adolescents'],
            ['Neurologist']
        ];
    }

    public function incorrectSpecializations(): array
    {
        return [
            [
                [
                    'name' => ['', ValidationConstants::BLANK_VALUE],
                    'description' => ['some text', ValidationConstants::SHORT_VALUE_10]
                ],
            ]
        ];
    }

    /**
     * @dataProvider correctSpecializations
     */
    public function testCreateSpecializationDto_withCorrectSpecializations(string $name, string $description = null): void
    {
        $dto = new CreateSpecializationDto();
        $dto->setName($name);
        if ($description)
            $dto->setDescription($description);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectSpecializations
     */
    public function testCreateSpecializationDto_withIncorrectSpecializations(array $data): void
    {
        $dto = new CreateSpecializationDto();
        $dto->setName($data['name'][0]);
        if (isset($data['description'][0]))
            $dto->setDescription($data['description'][0]);

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