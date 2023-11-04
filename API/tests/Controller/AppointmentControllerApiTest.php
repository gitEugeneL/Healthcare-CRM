<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;
use DateTime;

class AppointmentControllerApiTest extends ApiTestCase
{
    private array $patients = ['p1@gmail.com', 'p2@gmail.com', 'p3@gmail.com', 'p4@gmail.com'];
    private array $doctors = ['d1@gmail.com', 'd2@gmail.com', 'd3@gmail.com', 'd4@gmail.com'];

    private function createValidDate(): string
    {
        $date = (new DateTime())->modify('+1 day');
        if ($date->format('N') == 6 || $date->format('N') == 7)
            $date->modify('+2 day');
        return $date->format('Y-m-d');
    }

    private function createAppointments(string $date): void
    {
        for ($i = 0; $i < count($this->patients); $i++) {
            $this->user['patient']['email'] = $this->patients[$i];
            $this->user['doctor']['email'] = $this->doctors[$i];
            $this->createUser('doctor', $this->accessToken('manager'));
            $this->createUser('patient');
            $patientAccessToken = $this->accessToken('patient');

            for ($j = 0; $j < 2; $j++) {
                $this->request(
                    method: 'POST',
                    uri: '/api/appointments',
                    accessToken: $patientAccessToken,
                    data: [
                        'doctorId' => $i + 2, // the database already has an existing doctor !!
                        'date' => $date,
                        'startTime' => "1{$j}:00"
                    ]
                );
            }
        }
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $patientAccessToken = $this->accessToken('patient');
        $data = [
            'doctorId' => 1,
            'date' => $this->createValidDate(),
            'startTime' => '08:00'
        ];
        $response = $this->request(
            method: 'POST',
            uri: '/api/appointments',
            accessToken: $patientAccessToken,
            data: $data
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($data['date'], $responseData['date']);
        $this->assertSame($data['startTime'], $responseData['start']);
        $this->assertSame($this->user['doctor']['email'], $responseData['doctor']['email']);
        $this->assertSame($this->user['patient']['email'], $responseData['patient']['email']);
    }

    public function testCreate_withInvalidDate_returnsNotFound(): void
    {
        // edit doctor config
        $doctorAccessToken = $this->accessToken('doctor');
        $this->request(
            method: 'PUT',
            uri: '/api/doctor-config',
            accessToken: $doctorAccessToken,
            data: [
                'startTime' => '13:00',
                'endTime' => '15:00',
                'interval' => '15M',
                'workdays' => [6, 7],
            ]
        );
        // create appointment
        $patientAccessToken = $this->accessToken('patient');
        $data = [
            'doctorId' => 1,
            'date' => $this->createValidDate(),
            'startTime' => '08:00'
        ];
        $response = $this->request(
            method: 'POST',
            uri: '/api/appointments',
            accessToken: $patientAccessToken,
            data: $data
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Doctor does not work on this day', $response->getContent());
    }

    public function testCreate_withBusyTime_returnsNotFound(): void
    {
        // create appointment
        $patientAccessToken = $this->accessToken('patient');
        $data = [
            'doctorId' => 1,
            'date' => $this->createValidDate(),
            'startTime' => '15:00'
        ];
        for ($i = 0; $i < 2; $i++) {
            $response = $this->request(
                method: 'POST',
                uri: '/api/appointments',
                accessToken: $patientAccessToken,
                data: $data
            );
        }

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Doctor is not available at this time', $response->getContent());
    }

    public function testCreate_withInvalidTime_returnsNotFound(): void
    {
        // edit doctor config
        $doctorAccessToken = $this->accessToken('doctor');
        $this->request(
            method: 'PUT',
            uri: '/api/doctor-config',
            accessToken: $doctorAccessToken,
            data: [
                'startTime' => '08:00',
                'endTime' => '10:00',
                'interval' => '15M',
                'workdays' => [1, 2, 3, 4, 5],
            ]
        );
        // create appointment
        $patientAccessToken = $this->accessToken('patient');
        $data = [
            'doctorId' => 1,
            'date' => $this->createValidDate(),
            'startTime' => '12:00'
        ];
        $response = $this->request(
            method: 'POST',
            uri: '/api/appointments',
            accessToken: $patientAccessToken,
            data: $data
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Doctor is not available at this time', $response->getContent());
    }

    public function testShowFreeHours_validRequestAndValidData_returnsOk(): void
    {
        $patientAccessToken = $this->accessToken('patient');
        $date = $this->createValidDate();

        // create existing appointments 12:00 - 13:00
        $this->request(
            method: 'POST',
            uri: '/api/appointments',
            accessToken: $patientAccessToken,
            data: [
                'doctorId' => 1,
                'date' => $date,
                'startTime' => '12:00'
            ]
        );
        // find free times
        $response = $this->request(
            method: 'POST',
            uri: '/api/appointments/find-time',
            accessToken: $patientAccessToken,
            data: [
                'doctorId' => 1,
                'date' => $date,
            ]
        );
        $responseData = $this->decodeResponse($response);

        // Define the expected ыдщеы
        $expectedSlots = [
            '08:00 - 09:00',
            '09:00 - 10:00',
            '10:00 - 11:00',
            '11:00 - 12:00',
            '13:00 - 14:00',
            '14:00 - 15:00',
            '15:00 - 16:00',
            '16:00 - 17:00'
        ];
        // Check if all expected time slots are present except for 12:00 - 13:00
        $missingSlots = array_diff($expectedSlots, array_map(function ($slot) {
            return "{$slot['start']} - {$slot['end']}";
        }, $responseData));

        $this->assertSame(200, $response->getStatusCode());
        $this->assertEmpty($missingSlots);
    }

    public function testShowFreeHours_withChangedDoctorConfig_returnsOk(): void
    {
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        $date = $this->createValidDate();

        //change doctor config
        $this->request(
            method: 'PUT',
            uri: '/api/doctor-config',
            accessToken: $doctorAccessToken,
            data: [
                'startTime' => '12:00',
                'endTime' => '14:00',
                'interval' => '30M',
                'workdays' => [1, 2, 3, 4, 5],
            ]
        );
        // create existing appointments 12:00 - 12:30
        $this->request(
            method: 'POST',
            uri: '/api/appointments',
            accessToken: $patientAccessToken,
            data: [
                'doctorId' => 1,
                'date' => $date,
                'startTime' => '12:00'
            ]
        );
        // find free times
        $response = $this->request(
            method: 'POST',
            uri: '/api/appointments/find-time',
            accessToken: $patientAccessToken,
            data: [
                'doctorId' => 1,
                'date' => $date,
            ]
        );
        $responseData = $this->decodeResponse($response);

        $expectedSlots = [
            '12:30 - 13:00',
            '13:30 - 14:00',
        ];
        // Check if all expected time slots are present except for 12:00 - 13:00
        $missingSlots = array_diff($expectedSlots, array_map(function ($slot) {
            return "{$slot['start']} - {$slot['end']}";
        }, $responseData));

        $this->assertSame(200, $response->getStatusCode());
        $this->assertEmpty($missingSlots);
    }

    public function testShowForManager_withValidRequest_returnsOk(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        $response = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-manager?date={$date}",
            accessToken: $this->accessToken('manager')
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(count($this->patients) * 2, $responseData);
        for ($j = 0; $j < 2; $j++) {
            for ($i = 0; $i < count($this->patients); $i++) {
                $index = $i + $j * count($this->patients);
                if (isset($responseData[$index]['patient']['email'])) {
                    $this->assertSame($this->patients[$i], $responseData[$index]['patient']['email']);
                    $this->assertSame($this->doctors[$i], $responseData[$index]['doctor']['email']);
                }
            }
        }
    }

    public function testShowForDoctor_withValidData_returnsOk(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        $response = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-doctor?date={$date}",
            accessToken: $this->accessToken('doctor')
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(2, $responseData);
        foreach ($responseData as $elem)
            $this->assertSame($this->user['doctor']['email'], $elem['doctor']['email']);
    }

    public function testShowForPatient_withValidData_returnsOk(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        $response = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-patient?date={$date}",
            accessToken: $this->accessToken('patient')
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(2, $responseData);
        foreach ($responseData as $elem)
            $this->assertSame($this->user['patient']['email'], $elem['patient']['email']);
    }

    public function testFinalize_withValidData_returnsUpdated(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        // show doctor's appointments
        $appointmentsResponse = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-doctor?date={$date}",
            accessToken: $this->accessToken('doctor')
        );
        $appointmentsResponseData = $this->decodeResponse($appointmentsResponse);

        // finalize an appointment
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/appointments/{$appointmentsResponseData[0]['id']}/finalize",
            accessToken: $this->accessToken('doctor')
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['doctor']['email'], $responseData['doctor']['email']);
        $this->assertTrue($responseData['completed']);
    }

    public function testFinalize_withOverAppointment_returnsAlreadyExist(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        // show doctor's appointments
        $appointmentsResponse = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-doctor?date={$date}",
            accessToken: $this->accessToken('doctor')
        );
        $appointmentsResponseData = $this->decodeResponse($appointmentsResponse);

        // finalize an appointment
        for ($i = 0; $i < 2; $i++) {
            $response = $this->request(
                method: 'PATCH',
                uri: "/api/appointments/{$appointmentsResponseData[0]['id']}/finalize",
                accessToken: $this->accessToken('doctor')
            );
        }
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame('The appointment is already over', $response->getContent());
    }

    public function testFinalize_withSomeOneElseAppointment_returnsNoAccess(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        // finalize an appointment
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/appointments/2/finalize",
            accessToken: $this->accessToken('doctor')
        );

        $this->assertSame(403, $response->getStatusCode());
        $this->assertSame("Doctor doesn't have access", $response->getContent());
    }

    public function testCancel_withValidData_returnsUpdated(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        // show doctor's appointments
        $appointmentsResponse = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-doctor?date={$date}",
            accessToken: $this->accessToken('doctor')
        );
        $appointmentsResponseData = $this->decodeResponse($appointmentsResponse);

        // finalize an appointment
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/appointments/{$appointmentsResponseData[0]['id']}/cancel",
            accessToken: $this->accessToken('doctor')
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['doctor']['email'], $responseData['doctor']['email']);
        $this->assertTrue($responseData['canceled']);
    }

    public function testCancel_withCanceledAppointment_returnsAlreadyExist(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        // show doctor's appointments
        $appointmentsResponse = $this->request(
            method: 'GET',
            uri: "/api/appointments/show-for-doctor?date={$date}",
            accessToken: $this->accessToken('doctor')
        );
        $appointmentsResponseData = $this->decodeResponse($appointmentsResponse);

        // finalize an appointment
        for ($i = 0; $i < 2; $i++) {
            $response = $this->request(
                method: 'PATCH',
                uri: "/api/appointments/{$appointmentsResponseData[0]['id']}/cancel",
                accessToken: $this->accessToken('doctor')
            );
        }
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame('The appointment is already canceled', $response->getContent());
    }

    public function testCancel_withSomeOneElseAppointment_returnsNoAccess(): void
    {
        $date = $this->createValidDate();
        $this->createAppointments($date);

        // finalize an appointment
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/appointments/2/cancel",
            accessToken: $this->accessToken('doctor')
        );

        $this->assertSame(403, $response->getStatusCode());
        $this->assertSame("Doctor doesn't have access", $response->getContent());
    }
}