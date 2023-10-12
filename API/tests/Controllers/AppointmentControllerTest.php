<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;
use DateTime;

class AppointmentControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $doctor = $this->decodeResponse($this->createDoctor());
        $patient = $this->decodeResponse($this->createPatient());
        $patientAccessToken = $this->login($this->patient['email'], $this->patient['password']);
        $data = [
            'doctorId' => $doctor['id'],
            'date' => (new DateTime())->modify('+1 day')->format('Y-m-d'),
            'startTime' => '08:00'
        ];

        $response = $this->post(
            uri: '/api/appointment/create',
            accessToken: $patientAccessToken,
            data: $data
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($data['date'], $responseData['date']);
        $this->assertSame($data['startTime'], $responseData['start']);
        $this->assertSame($doctor['email'], $responseData['doctor']['email']);
        $this->assertSame($patient['email'], $responseData['patient']['email']);
    }



    //todo showFreeHours

    // todo show-for-manager

    // todo show-for-doctor'

    // todo show-for-patient

    // todo finalize

    // todo cancel
}