<?php

namespace App\Tests;

class DoctorControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->doctor['email'], json_decode($response->getContent(), true)['email']);
    }

    public function testCreate_withExistentDoctor_returnsAlreadyExist(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/doctor/create',
                data: $this->doctor,
                accessToken: $managerAccessToken
            );
        }
        $this->assertSame(422, $response->getStatusCode());
    }

    public function testShow_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 20; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->post(
                uri: '/api/doctor/create',
                data: $this->doctor,
                accessToken: $managerAccessToken
            );
        }
        $response = $this->get(
            uri: '/api/doctor/show?page=1',
            accessToken: $managerAccessToken
        );

        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
    }

    public function testShowOne_validId_returnsOk(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        $response = $this->get(
            uri: '/api/doctor/show/1',
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['id']);
        $this->assertSame($this->doctor['email'], $responseData['email']);
    }

    public function testShowOne_invalidId_returnsNotFound(): void
    {
        $id = 1;
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->get(
            uri: "/api/doctor/show/{$id}",
            accessToken: $managerAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("User id: {$id} not found", $response->getContent());
    }

    public function testUpdateStatus_withValidStatus_returnsUpdated(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        $response = $this->patch(
            uri: '/api/doctor/update-status',
            data: [
                'doctorId' => 1,
                'status' => 'DISABLED'
            ],
            accessToken: $managerAccessToken
        );
        $this->assertSame(204, $response->getStatusCode());
    }

    public function testUpdateStatus_withInvalidDoctorId_returnsNotFound(): void
    {
        $doctorId = 1;
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->patch(
            uri: '/api/doctor/update-status',
            data: [
                'doctorId' => $doctorId,
                'status' => 'DISABLED'
            ],
            accessToken: $managerAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Doctor id:{$doctorId} not found", $response->getContent());
    }

    public function testUpdateStatus_withInvalidStatus_returnsAlreadyUpdated(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        $response = $this->patch(
            uri: '/api/doctor/update-status',
            data: [
                'doctorId' => 1,
                'status' => 'ACTIVE'
            ],
            accessToken: $managerAccessToken
        );
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("Status has already been updated", $response->getContent());
    }

    public function testShowBySpecialization_withValidData_returnOk(): void
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
        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(5, $responseData['items']);
        foreach ($responseData['items'] as $doctor) {
            $this->assertSame('dentist', $doctor['specializations'][0]);
        }
    }
}