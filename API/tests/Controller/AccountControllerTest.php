<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;
use Symfony\Component\HttpFoundation\Response;

class AccountControllerTest extends ApiTestCase
{
    private function getInfo(string $token): Response
    {
        return $this->request(
            method: 'GET',
            uri: '/api/account/info',
            accessToken: $token
        );
    }

    public function testInfo_withValidManager_ReturnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->getInfo($managerAccessToken);
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['manager']['email'], $responseData['email']);
        $this->assertSame($this->user['manager']['lastName'], $responseData['lastName']);
        $this->assertSame($this->user['manager']['firstName'], $responseData['firstName']);
    }

    public function testInfo_withValidDoctor_ReturnsOk(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $response = $this->getInfo($doctorAccessToken);
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['doctor']['email'], $responseData['email']);
        $this->assertSame($this->user['doctor']['lastName'], $responseData['lastName']);
        $this->assertSame($this->user['doctor']['firstName'], $responseData['firstName']);
    }

    public function testInfo_withValidPatient_ReturnsOk(): void
    {
        $patientAccessToken = $this->accessToken('patient');

        $response = $this->getInfo($patientAccessToken);
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['patient']['email'], $responseData['email']);
        $this->assertSame($this->user['patient']['lastName'], $responseData['lastName']);
        $this->assertSame($this->user['patient']['firstName'], $responseData['firstName']);
    }

    public function testInfo_withInvalidUser_ReturnsOk(): void
    {
        $response = $this->getInfo('invalid token');
        $responseData = $this->decodeResponse($response);

        $this->assertSame(401, $response->getStatusCode());
        $this->assertSame('JWT Token not found', $responseData['message']);
    }
}