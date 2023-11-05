<?php

namespace App\Tests\DtoValidator\Appointment;

use App\Dto\Appointment\RequestAppointmentDto;
use App\Tests\DtoTestCase;
use DateTime;

class RequestAppointmentDtoTest extends DtoTestCase
{
    public function correctAppointments(): array
    {
        return [
            [25, (new DateTime())->modify('+1 day')->format('Y-m-d'), '09:00'],
            ['68', (new DateTime())->modify('+5 day')->format('Y-m-d'), '10:15'],
            [256, (new DateTime())->modify('+3 day')->format('Y-m-d'), '12:30'],
            ['999', (new DateTime())->modify('+6 day')->format('Y-m-d'), '13:45'],
            [98756, (new DateTime())->modify('+30 day')->format('Y-m-d')],
        ];
    }

    public function incorrectAppointments(): array
    {
        $blankValue = 'This value should not be blank.';
        $incorrectId = 'must be an integer and greater than 0';
        $invalidDate = 'Invalid date. Please select a future date or date should not be later than +1 month.';
        $invalidDateFormat = 'Invalid format. Date must be Y-m-d (1999-12-31)';
        $invalidTimeFormat = 'Incorrect time format (07:00|15|30|45 to 16:00|15|30|45)';

        return [
            [
                [
                    'doctorId' => ['', $blankValue],
                    'date' => ['', $blankValue],
                ]
            ],
            [
                [
                    'doctorId' => ['-10', "\"-10\" {$incorrectId}"],
                    'date' => [(new DateTime())->format('Y-m-d'), $invalidDate],
                    'startTime' => ['time', $invalidTimeFormat]
                ],
            ],
            [
                [
                    'doctorId' => ['0', "\"0\" {$incorrectId}"],
                    'date' => [(new DateTime())->modify('+32 day')->format('Y-m-d'), $invalidDate],
                    'startTime' => ['05:00',  $invalidTimeFormat]
                ],
            ],
            [
                [
                    'doctorId' => ['ASD', "\"ASD\" {$incorrectId}"],
                    'date' => ['12-2056-89', $invalidDateFormat],
                    'startTime' => ['08:02', $invalidTimeFormat]
                ],
            ]
        ];
    }

    /**
     * @dataProvider correctAppointments
     */
    public function testRequestAppointmentDto_withCorrectAppointments(
        int|string $doctorId, string $date, string $startTime = null
    ): void
    {
        $dto = new RequestAppointmentDto();
        $dto->setDoctorId($doctorId);
        $dto->setDate($date);
        if ($startTime)
            $dto->setStartTime($startTime);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectAppointments
     */
    public function testRequestAppointmentDto_withIncorrectAppointments(array $data): void
    {
        $dto = new RequestAppointmentDto();
        $dto->setDoctorId($data['doctorId'][0]);
        $dto->setDate($data['date'][0]);
        if (isset($data['startTime']))
            $dto->setStartTime($data['startTime'][0]);

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