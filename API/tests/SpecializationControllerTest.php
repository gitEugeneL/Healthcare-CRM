<?php

namespace App\Tests;

class SpecializationControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $specialization = ['name' => '   tEstSpEcializations '];
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->post(
            uri: '/api/specialization/create',
            data: $specialization,
            accessToken: $managerAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame(
            strtolower(trim($specialization['name'])), json_decode($response->getContent(), true)['name']);
    }

    public function testCreate_withInvalidData_returnsAlreadyExists(): void
    {
        $specialization = ['name' => 'specialization1'];
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/specialization/create',
                data: $specialization,
                accessToken: $managerAccessToken
            );
        }
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("Specialization {$specialization['name']} already exists", $response->getContent());
    }

    public function testShow_withValidUser_returnsOk(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 5; $i++) {
            $specialization = ['name' => "specialization{$i}"];
            $this->post(
                uri: '/api/specialization/create',
                data: $specialization,
                accessToken: $managerAccessToken
            );
        }
        $response = $this->get(
            uri: '/api/specialization/show',
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame('specialization0', $responseData[0]['name']);
        $this->assertSame('specialization4', $responseData[4]['name']);
    }

    public function includeDoctor_withValidData_returnsUpdated(): void
    {
        $specialization = ['name' => 'dentist'];
        $managerAccessToken = $this->createAndLoginManager();
        $this->post(
            uri: '/api/specialization/create',
            data: $specialization,
            accessToken: $managerAccessToken
        );
        for ($i = 1; $i <= 5; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->post(
                uri: '/api/doctor/create',
                data: $this->doctor,
                accessToken: $managerAccessToken
            );
            $this->patch(
                uri: '/api/specialization/include-doctor',
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ],
                accessToken:  $managerAccessToken
            );
        }
        $response = $this->get(
            uri: "/api/doctor/show-by-specialization/{$specialization['name']}",
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(201, $response->getStatusCode());
        $this->assertCount(5, $responseData['items']);
        foreach ($responseData['items'] as $doctor) {
            $this->assertSame('dentist', $doctor['specializations'][0]);
        }
    }

    public function excludeDoctor_withValidData_returnsUpdated(): void
    {
        $specialization = ['name' => 'dentist'];
        $managerAccessToken = $this->createAndLoginManager();
        $this->post(
            uri: '/api/specialization/create',
            data: $specialization,
            accessToken: $managerAccessToken
        );
        for ($i = 1; $i <= 5; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->post(
                uri: '/api/doctor/create',
                data: $this->doctor,
                accessToken: $managerAccessToken
            );
            $this->patch(
                uri: '/api/specialization/include-doctor',
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ],
                accessToken:  $managerAccessToken
            );
            $this->patch(
                uri: '/api/specialization/exclude-doctor',
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ],
                accessToken:  $managerAccessToken
            );
        }
        $response = $this->get(
            uri: "/api/doctor/show-by-specialization/{$specialization['name']}",
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(201, $response->getStatusCode());
        $this->assertCount(0, $responseData['items']);

    }
}