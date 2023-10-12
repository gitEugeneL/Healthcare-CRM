<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;

class DoctorControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $createDoctorResponse = $this->createDoctor();

        $this->assertSame(201, $createDoctorResponse->getStatusCode());
        $this->assertSame($this->doctor['email'], $this->decodeResponse($createDoctorResponse)['email']);
    }

    public function testCreate_withExistentDoctor_returnsAlreadyExist(): void
    {
        for ($i = 0; $i < 2; $i++) {
            $response = $this->createDoctor();
        }
        $this->assertSame(422, $response->getStatusCode());
    }

    public function testShow_validRequest_returnsOk(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        for ($i = 0; $i < 20; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->createDoctor();
        }
        $response = $this->get(
            uri: '/api/doctor/show?page=1',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
    }

    public function testShowOne_validId_returnsOk(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $doctor = $this->decodeResponse($this->createDoctor());

        $response = $this->get(
            uri: "/api/doctor/show/{$doctor['id']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($doctor['id'], $responseData['id']);
        $this->assertSame($doctor['email'], $responseData['email']);
    }

    public function testShowOne_invalidId_returnsNotFound(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $response = $this->get(
            uri: '/api/doctor/show/777',
            accessToken: $managerAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('User id: 777 not found', $response->getContent());
    }

    public function testUpdateStatus_withValidStatus_returnsUpdated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $doctor = $this->decodeResponse($this->createDoctor());

        $response = $this->patch(
            uri: '/api/doctor/update-status',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => $doctor['id'],
                'status' => 'DISABLED'
            ]
        );

        $this->assertSame(200, $response->getStatusCode());
    }

    public function testUpdateStatus_withInvalidDoctorId_returnsNotFound(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $response = $this->patch(
            uri: '/api/doctor/update-status',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => 777,
                'status' => 'DISABLED'
            ]
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Doctor id:777 not found', $response->getContent());
    }

    public function testUpdateStatus_withInvalidStatus_returnsAlreadyUpdated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $doctor = $this->decodeResponse($this->createDoctor());

        $response = $this->patch(
            uri: '/api/doctor/update-status',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => $doctor['id'],
                'status' => 'ACTIVE'
            ]
        );
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame('Status has already been updated', $response->getContent());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $this->decodeResponse($this->createDoctor());
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);
        $updateData = [
            'firstName' => "new doctor's name",
            'description' => 'new description',
            'education' => 'new education',
            'phone' => '+48999888999'
        ];

        $response = $this->patch(
            uri: '/api/doctor/update',
            accessToken: $doctorAccessToken,
            data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['description'], $responseData['description']);
        $this->assertSame($updateData['education'], $responseData['education']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($this->doctor['email'], $responseData['email']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $this->decodeResponse($this->createDoctor());
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);

        $response = $this->patch(
            uri: '/api/doctor/update',
            accessToken: $doctorAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Nothing to change', $response->getContent());
    }

    public function testShowBySpecialization_withValidData_returnOk(): void
    {
        $specialization = ['name' => 'dentist'];
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $this->post(
            uri: '/api/specialization/create',
            accessToken: $managerAccessToken,
            data: $specialization
        );
        for ($i = 1; $i <= 5; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->createDoctor();

            $this->patch(
                uri: '/api/specialization/include-doctor',
                accessToken: $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ]
            );
        }
        $response = $this->get(
            uri: "/api/doctor/show-by-specialization/{$specialization['name']}",
            accessToken: $managerAccessToken
        );

        $responseData = $this->decodeResponse($response);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(5, $responseData['items']);
        foreach ($responseData['items'] as $doctor) {
            $this->assertSame('dentist', $doctor['specializations'][0]['name']);
        }
    }
}