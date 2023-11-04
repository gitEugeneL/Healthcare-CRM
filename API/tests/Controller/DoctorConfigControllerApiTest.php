<?php

namespace App\Tests\Controller;

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
        $this->assertSame('incorrect time format (07:00 to 16:00)', $requestData['startTime']);
        $this->assertSame('incorrect time format (08:00 to 17:00)', $requestData['endTime']);
        $this->assertSame('incorrect interval available: 1H or 15M or 30M or 45M', $requestData['interval']);
        $this->assertSame('incorrect workdays array. available: [1, 2, 3, 4, 5, 6, 7]', $requestData['workdays']);
    }
}