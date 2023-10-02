<?php

namespace App\Tests;


class PatientControllerTest extends TestCase
{
    public function testCreatePatient_withValidData_returnsCreated(): void
    {
        $response = $this->post(
            uri: '/api/patient/create',
            data: $this->patient
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame($this->patient['email'], $responseData['email']);
        $this->assertSame($this->patient['firstName'], $responseData['firstName']);
        $this->assertSame($this->patient['lastName'], $responseData['lastName']);
        $this->assertSame(201, $response->getStatusCode());
    }

    public function testCreate_withExistentPatient_returnsAlreadyExist(): void
    {
        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/patient/create',
                data: $this->patient
            );
        }
        $this->assertSame(422, $response->getStatusCode());
    }

    public function testShow_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 15; $i++) {
            $this->patient['email'] = "patient{$i}@paatient.com";
            $this->post(
                uri: '/api/patient/create',
                data: $this->patient
            );
        }
        $response = $this->get(
            uri: '/api/patient/show?page=1',
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
    }

    public function testShowOne_withValidId_returnsOK(): void
    {
        $this->post(
            uri: '/api/patient/create',
            data: $this->patient
        );
        $patientId = 1;
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->get(
            uri: "/api/patient/show/{$patientId}",
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($patientId, $responseData['id']);
        $this->assertSame($this->patient['email'], $responseData['email']);
    }

    public function testShowOne_withInvalidId_returnsNotFound(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->get(
            uri: '/api/patient/show/1',
            accessToken: $managerAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('patient not found', $response->getContent());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $updateData = [
            'firstName' => 'updated patient',
            'phone' => '123456789',
            'pesel' => '00000000000',
            'dateOfBirth' => '2030-12-31',
            'insurance' => 'nfz'
        ];
        $patientAccessToken = $this->createAndLoginPatient();
        $response = $this->patch(
          uri: '/api/patient/update',
          accessToken: $patientAccessToken,
          data: $updateData
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($updateData['pesel'], $responseData['pesel']);
        $this->assertSame($this->patient['email'], $responseData['email']);
        $this->assertSame($this->patient['lastName'], $responseData['lastName']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $patientAccessToken = $this->createAndLoginPatient();
        $response = $this->patch(
            uri: '/api/patient/update',
            accessToken: $patientAccessToken,
        );
        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Nothing to change', $response->getContent());
    }
}