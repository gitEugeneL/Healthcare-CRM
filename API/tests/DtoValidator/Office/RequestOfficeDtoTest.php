<?php

namespace App\Tests\DtoValidator\Office;

use App\Constant\ValidationConstants;
use App\Dto\Office\RequestOfficeDto;
use App\Tests\DtoTestCase;

class RequestOfficeDtoTest extends DtoTestCase
{
    public function correctOffices(): array
    {
        return [
            ['Pediatrics', 105],
            ['Cardiology', '2568'],
        ];
    }

    public function incorrectOffices(): array
    {
        return [
            [
                [
                    'name' => ['', ValidationConstants::BLANK_VALUE],
                    'number' => ['', ValidationConstants::BLANK_VALUE]
                ]
            ],
            [
                [
                    'name' => ['', ValidationConstants::BLANK_VALUE],
                    'number' => ['-25', '"-25"'.' '.ValidationConstants::INCORRECT_ID]
                ]
            ],
        ];
    }

    /**
     * @dataProvider correctOffices
     */
    public function testRequestOfficesDto_withCorrectOffices(string $name, string|int $number): void
    {
        $dto = new RequestOfficeDto();
        $dto->setName($name);
        $dto->setNumber($number);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectOffices
     */
    public function testRequestOfficesDto_withIncorrectOffices(array $data): void
    {
        $dto = new RequestOfficeDto();
        $dto->setName($data['name'][0]);
        $dto->setNumber($data['number'][0]);

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