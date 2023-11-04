<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;
use Symfony\Component\HttpFoundation\Response;

class OfficeControllerApiTest extends ApiTestCase
{
    private array $office = [
        'number' => 104,
        'name' => 'surgical office'
    ];

    private function createNewOffice(string $managerAccessToken): Response
    {
        return $this->request(
            method: 'POST',
            uri: '/api/offices',
            accessToken: $managerAccessToken,
            data: $this->office
        );
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $response = $this->createNewOffice($managerAccessToken);
        $responseData = $this->decodeResponse($response);

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->office['number'], $responseData['number'] );
        $this->assertSame(ucfirst($this->office['name']), $responseData['name']);
        $this->assertFalse($responseData['isAvailable']);
    }

    public function testCreate_withExistingOffice_returnsAlreadyExists(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        for ($i = 0; $i < 2; $i++)
            $response = $this->createNewOffice($managerAccessToken);

        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("Office number: {$this->office['number']} already exists", $response->getContent());
    }

    public function testShow_withValidRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        for ($i = 1; $i < 10; $i++) {
            $this->office['number'] = $i;
            $this->createNewOffice($managerAccessToken);
        }

        $response = $this->request(
            method: 'GET',
            uri: '/api/offices',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        for ($i = 1; $i < count($responseData); $i++) {
            $this->assertSame(ucfirst($this->office['name']), $responseData[$i - 1]['name']);
            $this->assertFalse($responseData[$i - 1]['isAvailable']);
            $this->assertSame($i, $responseData[$i - 1]['number']);
        }
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $createResponse = $this->createNewOffice($managerAccessToken);
        $createResponseData = $this->decodeResponse($createResponse);

        $data = [
            'number' => $createResponseData['number'],
            'name' => 'new office name'
        ];
        $response = $this->request(
            method: 'PATCH',
            uri: '/api/offices',
            accessToken: $managerAccessToken,
            data: $data
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(ucfirst($data['name']), $responseData['name']);
        $this->assertSame($createResponseData['number'], $responseData['number']);
        $this->assertFalse($responseData['isAvailable']);
    }

    public function testUpdate_withInvalidOffice_returnsNotfound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $data = [
            'number' => 777,
            'name' => 'new office name'
        ];
        $response = $this->request(
            method: 'PATCH',
            uri: '/api/offices',
            accessToken: $managerAccessToken,
            data: $data
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Office number: {$data['number']} doesn't exist", $response->getContent());
    }

    public function testChangeStatus_withValidOffice_returnsUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $this->createNewOffice($managerAccessToken);

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/offices/{$this->office['number']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertTrue($responseData['isAvailable']);
        $this->assertSame(ucfirst($this->office['name']), $responseData['name']);
        $this->assertSame($this->office['number'], $responseData['number']);
    }

    public function testChangeStatus_withInvalidOffice_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $number = 777;
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/offices/{$number}",
            accessToken: $managerAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Office number: {$number} doesn't exist", $response->getContent());
    }

    public function testDelete_withValidOffice_returnsDeleted(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $this->createNewOffice($managerAccessToken);
        $response = $this->request(
            method: 'DELETE',
            uri: "/api/offices/{$this->office['number']}",
            accessToken: $managerAccessToken
        );
        $this->assertSame(204, $response->getStatusCode());
    }

    public function testDelete_withInvalidOffice_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $number = 777;
        $response = $this->request(
            method: 'DELETE',
            uri: "/api/offices/{$number}",
            accessToken: $managerAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Office number: {$number} doesn't exist", $response->getContent());
    }
}