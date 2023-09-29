<?php

namespace App\Tests;

class ManagerControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $response = $this->post(
            uri: '/api/manager/create',
            accessToken: $adminAccessToken,
            data: $this->manager
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->manager['email'], json_decode($response->getContent(), true)['email']);
    }

    public function testCreate_witExistentManager_returnsAlreadyExist(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/manager/create',
                accessToken: $adminAccessToken,
                data: $this->manager
            );
        }
        $this->assertSame(422, $response->getStatusCode());
    }

    public function testInfo_withValidManager_returnsOk(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->get(
            uri: '/api/manager/info',
            accessToken: $managerAccessToken
        );

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->manager['email'], json_decode($response->getContent(), true)['email']);
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
        $managerAccessToken = $this->createAndLoginManager();
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
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['lastName'], $responseData['lastName']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($updateData['position'], $responseData['position']);
        $this->assertSame($this->manager['email'], $responseData['email']);
    }
}