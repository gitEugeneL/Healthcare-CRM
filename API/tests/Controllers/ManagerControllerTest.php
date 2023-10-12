<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;

class ManagerControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $createManagerResponse = $this->createManager();
        $responseData = $this->decodeResponse($createManagerResponse);

        $this->assertSame(201, $createManagerResponse->getStatusCode());
        $this->assertSame($this->manager['email'], $responseData['email']);
    }

    public function testCreate_witExistentManager_returnsAlreadyExist(): void
    {
        for ($i = 0; $i < 2; $i++)
            $response = $this->createManager();

        $this->assertSame(422, $response->getStatusCode());
    }

    public function testInfo_withValidManager_returnsOk(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $response = $this->get(
            uri: '/api/manager/info',
            accessToken: $managerAccessToken
        );

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->manager['email'], $this->decodeResponse($response)['email']);
    }

    public function testInfo_withInvalidUser_returnsUnauthorized(): void
    {
        $response = $this->get(
            uri: '/api/manager/info',
            accessToken: 'invalidToken'
        );

        $this->assertSame(401, $response->getStatusCode());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $updateData = [
            'firstName' => 'new first name',
            'lastName' => 'new last name',
            'phone' => '+48999888999',
            'position' => 'the best manager'
        ];

        $response = $this->patch(
            uri: '/api/manager/update',
            accessToken: $managerAccessToken,
            data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['lastName'], $responseData['lastName']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($updateData['position'], $responseData['position']);
        $this->assertSame($this->manager['email'], $responseData['email']);
    }
}