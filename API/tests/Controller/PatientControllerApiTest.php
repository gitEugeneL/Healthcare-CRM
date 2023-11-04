<?php

namespace App\Tests\Controller;


use App\Tests\ApiTestCase;
use Symfony\Component\HttpFoundation\Response;

class PatientControllerApiTest extends ApiTestCase
{
    private function createNewPatient(): Response
    {
        $this->user['patient']['email'] = 'test@p.com';
        return $this->createUser('patient');
    }

    public function testCreatePatient_withValidData_returnsCreated(): void
    {
        $response = $this->createNewPatient();
        $responseData = $this->decodeResponse($response);
        $this->assertSame($this->user['patient']['email'], $responseData['email']);
        $this->assertSame(ucfirst($this->user['patient']['firstName']), $responseData['firstName']);
        $this->assertSame(ucfirst($this->user['patient']['lastName']), $responseData['lastName']);
        $this->assertSame(201, $response->getStatusCode());
    }

    public function testCreate_withExistentPatient_returnsAlreadyExist(): void
    {
        $response = $this->createUser('patient');
        $this->assertSame(422, $response->getStatusCode());
    }

    public function testShow_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        for ($i = 0; $i < 15; $i++) {
            $this->user['patient']['email'] = "patient{$i}@patient.com";
            $this->createUser('patient');
        }
        $response = $this->request(
            method: 'GET',
            uri: '/api/patients/?page=1',
            accessToken: $managerAccessToken
        );

        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
        $this->assertCount(10, $responseData['items']);
    }

    public function testShowOne_withValidId_returnsOK(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $response = $this->request(
            method: 'GET',
            uri: '/api/patients/1',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['id']);
        $this->assertSame($this->user['patient']['email'], $responseData['email']);
    }

    public function testShowOne_withInvalidId_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $response = $this->request(
            method: 'GET',
            uri: '/api/patients/777',
            accessToken: $managerAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Patient id: 777 doesn't exist", $response->getContent());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $patientAccessToken = $this->accessToken('patient');
        $updateData = [
            'firstName' => 'Updated patient',
            'phone' => '123456789',
            'pesel' => '00000000000',
            'dateOfBirth' => '2030-12-31',
            'insurance' => 'nfz'
        ];

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/patients',
            accessToken: $patientAccessToken,
            data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($updateData['pesel'], $responseData['pesel']);
        $this->assertSame($this->user['patient']['email'], $responseData['email']);
        $this->assertSame($this->user['patient']['lastName'], $responseData['lastName']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $patientAccessToken = $this->accessToken('patient');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/patients',
            accessToken: $patientAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Nothing to change', $response->getContent());
    }
}