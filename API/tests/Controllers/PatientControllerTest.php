<?php

namespace App\Tests\Controllers;


use App\Tests\TestCase;

class PatientControllerTest extends TestCase
{
    public function testCreatePatient_withValidData_returnsCreated(): void
    {
        $response = $this->createPatient();
        $responseData = $this->decodeResponse($response);

        $this->assertSame($this->patient['email'], $responseData['email']);
        $this->assertSame($this->patient['firstName'], $responseData['firstName']);
        $this->assertSame($this->patient['lastName'], $responseData['lastName']);
        $this->assertSame(201, $response->getStatusCode());
    }

    public function testCreate_withExistentPatient_returnsAlreadyExist(): void
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

        for ($i = 0; $i < 15; $i++) {
            $this->patient['email'] = "patient{$i}@patient.com";
            $this->createPatient();
        }
        $response = $this->get(
            uri: '/api/patient/show?page=1',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
    }

    public function testShowOne_withValidId_returnsOK(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $patient = $this->decodeResponse($this->createPatient());

        $response = $this->get(
            uri: "/api/patient/show/{$patient['id']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($patient['id'], $responseData['id']);
        $this->assertSame($this->patient['email'], $responseData['email']);
    }

    public function testShowOne_withInvalidId_returnsNotFound(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $response = $this->get(
            uri: '/api/patient/show/777',
            accessToken: $managerAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('patient not found', $response->getContent());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $this->createPatient();
        $patientAccessToken = $this->login($this->patient['email'], $this->patient['password']);
        $updateData = [
            'firstName' => 'updated patient',
            'phone' => '123456789',
            'pesel' => '00000000000',
            'dateOfBirth' => '2030-12-31',
            'insurance' => 'nfz'
        ];

        $response = $this->patch(
          uri: '/api/patient/update',
          accessToken: $patientAccessToken,
          data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($updateData['pesel'], $responseData['pesel']);
        $this->assertSame($this->patient['email'], $responseData['email']);
        $this->assertSame($this->patient['lastName'], $responseData['lastName']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $this->createPatient();
        $patientAccessToken = $this->login($this->patient['email'], $this->patient['password']);

        $response = $this->patch(
            uri: '/api/patient/update',
            accessToken: $patientAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Nothing to change', $response->getContent());
    }
}