<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;
use Symfony\Component\HttpFoundation\Response;

class DoctorControllerApiTest extends ApiTestCase
{
    private function createNewDoctor(string $managerAccessToken): Response
    {
        $this->user['doctor']['email'] = 'test@d.com';
        return $this->createUser('doctor', $managerAccessToken);
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $response = $this->createNewDoctor($this->accessToken('manager'));
        $responseData = $this->decodeResponse($response);

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->user['doctor']['email'], $responseData['email']);
    }

    public function testCreate_withExistentDoctor_returnsAlreadyExist(): void
    {
        $response = $this->createUser('doctor', $this->accessToken('manager'));
        $this->assertSame(422, $response->getStatusCode());
    }

    public function testShow_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        for ($i = 1; $i < 20; $i++) {
            $this->user['doctor']['email'] = "doctor{$i}@doctor.com";
            $this->createUser('doctor', $managerAccessToken);
        }
        $response = $this->request(
            method: 'GET',
            uri: '/api/doctors/?page=1',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
    }

    public function testShowOne_validId_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->request(
            method: 'GET',
            uri: '/api/doctors/1',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['id']);
        $this->assertSame($this->user['doctor']['email'], $responseData['email']);
    }

    public function testShowOne_invalidId_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->request(
            method: 'GET',
            uri: '/api/doctors/777',
            accessToken: $managerAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Doctor id: 777 doesn't exist", $response->getContent());
    }

    public function testUpdateStatus_withValidStatus_returnsUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/doctors/update-status',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => 1,
                'status' => 'DISABLED'
            ]
        );
        $this->assertSame(200, $response->getStatusCode());
    }

    public function testUpdateStatus_withInvalidDoctorId_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/doctors/update-status',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => 777,
                'status' => 'DISABLED'
            ]
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Doctor id: 777 doesn't exist", $response->getContent());
    }

    public function testUpdateStatus_withInvalidStatus_returnsAlreadyUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/doctors/update-status',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => 1,
                'status' => 'ACTIVE'
            ]
        );
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame('Status has already been updated', $response->getContent());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $updateData = [
            'firstName' => "New doctor's name",
            'description' => 'New description',
            'education' => 'New education',
            'phone' => '+48999888999'
        ];

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/doctors',
            accessToken: $doctorAccessToken,
            data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['description'], $responseData['description']);
        $this->assertSame($updateData['education'], $responseData['education']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($this->user['doctor']['email'], $responseData['email']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/doctors',
            accessToken: $doctorAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Nothing to change', $response->getContent());
    }

    public function testShowBySpecialization_withValidData_returnOk(): void
    {
        $specialization = ['name' => 'dentist'];
        $managerAccessToken = $this->accessToken('manager');

        $this->request(
            method: 'POST',
            uri: '/api/specializations',
            accessToken: $managerAccessToken,
            data: $specialization
        );

        for ($i = 2; $i <= 6; $i++) {
            $this->user['doctor']['email'] = "doctor{$i}@doctor.com";
            $this->createUser('doctor');

            $this->request(
                method: 'PATCH',
                uri: '/api/specializations/include-doctor',
                accessToken: $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ]
            );
        }
        $response = $this->request(
            method: 'GET',
            uri: "/api/doctors/show-by-specialization/{$specialization['name']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(5, $responseData['items']);
        foreach ($responseData['items'] as $doctor) {
            $this->assertSame(ucfirst(strtolower(trim($specialization['name']))), $doctor['specializations'][0]['name']);
        }
    }
}