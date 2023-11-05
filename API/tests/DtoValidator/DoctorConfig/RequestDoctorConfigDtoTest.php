<?php

namespace App\Tests\DtoValidator\DoctorConfig;

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
        $blankValue = 'This value should not be blank.';
        $startTimeValue = 'Incorrect time format. Available: (07:00 to 16:00)';
        $endTimeValue = 'Incorrect time format. Available: (08:00 to 17:00)';
        $intervalValue = 'Incorrect interval. Available: 1H or 15M or 30M or 45M';
        $workdaysValue = 'Incorrect workdays array. Available: [1, 2, 3, 4, 5, 6, 7]';

        return [
            [
                [
                    'startTime' => ['', $blankValue],
                    'endTime' => ['', $blankValue],
                    'interval' => ['', $blankValue],
                    'workdays' => ['', $blankValue]
                ],
                [
                    'startTime' => ['15:15', $startTimeValue],
                    'endTime' => ['16:30', $endTimeValue],
                    'interval' => ['25M', $intervalValue],
                    'workdays' => ['monday, friday', $workdaysValue]
                ],
                [
                    'startTime' => ['04:00', $startTimeValue],
                    'endTime' => ['20:00', $endTimeValue],
                    'interval' => ['2h', $intervalValue],
                    'workdays' => [[-1, 'day'], $workdaysValue]
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