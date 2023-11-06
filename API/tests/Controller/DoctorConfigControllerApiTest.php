<?php

namespace App\Tests\Controller;

use App\Constant\ValidationConstants;
use App\Tests\ApiTestCase;

class DoctorConfigControllerApiTest extends ApiTestCase
{
    public function testConfig_withValidData_returnsUpdated(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');
        $data = [
            'startTime' => '10:00',
            'endTime' => '15:00',
            'interval' => '30M',
            'workdays' => [1, 2, 5],
        ];

        $request = $this->request(
            method: 'PUT',
            uri: '/api/doctor-config',
            accessToken: $doctorAccessToken,
            data: $data
        );
        $requestData = $this->decodeResponse($request);

        $this->assertSame(200, $request->getStatusCode());
        $this->assertSame($data['startTime'], $requestData['startTime']);
        $this->assertSame($data['endTime'], $requestData['endTime']);
        $this->assertSame($data['interval'], $requestData['interval']);
        $this->assertSame($data['workdays'], $requestData['workdays']);
        $this->assertSame(1, $requestData['doctorId']);
    }

    public function testConfig_withInvalidData_returnsInvalidData(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');
        $data = [
            'startTime' => '05:00',
            'endTime' => '20:00',
            'interval' => '2H',
            'workdays' => [1, 2, 6, 7, -1, 0, 10],
        ];

        $request = $this->request(
            method: 'PUT',
            uri: '/api/doctor-config',
            accessToken: $doctorAccessToken,
            data: $data
        );
        $requestData = $this->decodeResponse($request);

        $this->assertSame(422, $request->getStatusCode());
        $this->assertSame(ValidationConstants::INCORRECT_START_TIME, $requestData['startTime']);
        $this->assertSame(ValidationConstants::INCORRECT_END_TIME, $requestData['endTime']);
        $this->assertSame(ValidationConstants::INCORRECT_INTERVAL, $requestData['interval']);
        $this->assertSame(ValidationConstants::INCORRECT_WORKDAYS, $requestData['workdays']);
    }
}