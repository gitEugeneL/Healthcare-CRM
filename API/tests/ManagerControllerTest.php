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
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("User {$this->manager['email']} already exists", $response->getContent());
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
}