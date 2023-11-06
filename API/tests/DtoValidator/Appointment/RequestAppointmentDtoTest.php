<?php

namespace App\Tests\DtoValidator\Appointment;

use App\Constant\ValidationConstants;
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
        return [
            [
                [
                    'doctorId' => ['', ValidationConstants::BLANK_VALUE],
                    'date' => ['', ValidationConstants::BLANK_VALUE],
                ]
            ],
            [
                [
                    'doctorId' => ['-10', '"-10"'.' '.ValidationConstants::INCORRECT_ID],
                    'date' => [(new DateTime())->format('Y-m-d'), ValidationConstants::INVALID_DATE],
                    'startTime' => ['time', ValidationConstants::INVALID_TIME]
                ],
            ],
            [
                [
                    'doctorId' => ['0', '"0"'.' '.ValidationConstants::INCORRECT_ID],
                    'date' => [(new DateTime())->modify('+32 day')->format('Y-m-d'),
                        ValidationConstants::INVALID_DATE],
                    'startTime' => ['05:00',  ValidationConstants::INVALID_TIME]
                ],
            ],
            [
                [
                    'doctorId' => ['ASD', '"ASD"'.' '.ValidationConstants::INCORRECT_ID],
                    'date' => ['12-2056-89', ValidationConstants::INVALID_DATE_FORMAT],
                    'startTime' => ['08:02', ValidationConstants::INVALID_TIME]
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