<?php

namespace App\Tests;


use Symfony\Component\HttpFoundation\Response;


class ManagerControllerTest extends TestCase
{
    private array $admin = [
        'username' => 'admin@admin.com',
        'password' => 'admin!1A'
    ];

    private array $manager = [
        'lastName' => 'manager',
        'firstName' => 'manager',
        'email' => 'm@m.com',
        'password' => 'manager!1M',
        'phone' => '+00000000000'
    ];

    private function createManager(string $accessToken): Response
    {
        return $this->post(
            uri: '/api/manager/create',
            data: $this->manager,
            accessToken: $accessToken
        );
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $response = $this->createManager($adminAccessToken);
        $this->assertSame(201, $response->getStatusCode());
        $this->assertJson($response->getContent());
        $this->assertSame($this->manager['email'], json_decode($response->getContent(), true)['email']);
    }

    public function testCreate_witExistentManager_returnsAlreadyExist(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $this->createManager($adminAccessToken);

        $response = $this->post(
            uri: '/api/manager/create',
            data: $this->manager,
            accessToken: $adminAccessToken
        );
        $this->assertSame(409, $response->getStatusCode());
        $this->assertIsString($response->getContent());
        $this->assertSame("User {$this->manager['email']} already exists", $response->getContent());
    }

    public function testInfo_withValidManager_returnsOk(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $this->createManager($adminAccessToken);

        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
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