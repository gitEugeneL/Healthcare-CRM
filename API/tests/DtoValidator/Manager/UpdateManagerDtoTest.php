<?php

namespace App\Tests\DtoValidator\Manager;

use App\Dto\Manager\UpdateManagerDto;
use App\Tests\DtoTestCase;

class UpdateManagerDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            ['Marketing Manager'],
            ['Financial Manager'],
            ['Healthcare Manager']
        ];
    }

    public function incorrectData(): array
    {
        $positionLength = 'This value is too short. It should have 10 characters or more.';

        return [
            ['qwe', $positionLength],
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testUpdateManagerDto_withCorrectData(string $position): void
    {
        $dto = new UpdateManagerDto();
        $dto->setPosition($position);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectData
     */
    public function testUpdateManagerDto_withIncorrectData(string $value, string $error): void
    {
        $dto = new UpdateManagerDto();
        $dto->setPosition($value);

        $violations = $this->validator->validate($dto);
        $result = $this->inspect($violations);

        $this->assertSame($result['position'], $error);
    }
}