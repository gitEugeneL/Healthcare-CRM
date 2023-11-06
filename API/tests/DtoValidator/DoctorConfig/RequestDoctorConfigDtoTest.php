<?php

namespace App\Tests\DtoValidator\DoctorConfig;

use App\Constant\ValidationConstants;
use App\Dto\DoctorConfig\RequestDoctorConfigDto;
use App\Tests\DtoTestCase;

class RequestDoctorConfigDtoTest extends DtoTestCase
{
    public function correctConfigs(): array
    {
        return [
            ['07:00', '17:00', '1H', [1, 2, 3, 4, 5]],
            ['08:00', '12:00', '15M', [2, 1, 5]],
            ['15:00', '16:00', '30M', [5]],
            ['14:00', '16:00', '30M', [5, 4]]
        ];
    }

    public function incorrectConfigs(): array
    {
        return [
            [
                [
                    'startTime' => ['', ValidationConstants::BLANK_VALUE],
                    'endTime' => ['', ValidationConstants::BLANK_VALUE],
                    'interval' => ['', ValidationConstants::BLANK_VALUE],
                    'workdays' => ['', ValidationConstants::BLANK_VALUE]
                ]
            ],
            [
                [
                    'startTime' => ['15:15', ValidationConstants::INCORRECT_START_TIME],
                    'endTime' => ['16:30', ValidationConstants::INCORRECT_END_TIME],
                    'interval' => ['25M', ValidationConstants::INCORRECT_INTERVAL],
                    'workdays' => [['monday, friday'], ValidationConstants::INCORRECT_WORKDAYS]
                ],
            ],
            [
                [
                    'startTime' => ['04:00', ValidationConstants::INCORRECT_START_TIME],
                    'endTime' => ['20:00', ValidationConstants::INCORRECT_END_TIME],
                    'interval' => ['2h', ValidationConstants::INCORRECT_INTERVAL],
                    'workdays' => [[-1, 'day'], ValidationConstants::INCORRECT_WORKDAYS]
                ],
            ]
        ];
    }

    /**
     * @dataProvider correctConfigs
     */
    public function testRequestDoctorConfigDto_withCorrectConfigs(
        string $startTime, string $endTime, string $interval, array $workdays
    ): void
    {
        $dto = new RequestDoctorConfigDto();
        $dto->setStartTime($startTime);
        $dto->setEndTime($endTime);
        $dto->setInterval($interval);
        $dto->setWorkdays($workdays);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectConfigs
     */
    public function testRequestDoctorConfigDto_withIncorrectConfigs(array $data): void
    {
        $dto = new RequestDoctorConfigDto();
        $dto->setStartTime($data['startTime'][0]);
        $dto->setEndTime($data['endTime'][0]);
        $dto->setInterval($data['interval'][0]);
        $dto->setWorkdays($data['workdays'][0]);

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