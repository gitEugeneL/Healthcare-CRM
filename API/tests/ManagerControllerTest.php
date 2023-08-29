<?php

namespace App\Tests;

class ManagerControllerTest extends TestCase
{
    private array $admin = [
        'username' => 'admin@admin.com',
        'password' => 'admin!1A'
    ];
    private array $nonexistentManager = [
        'lastName' => 'manager',
        'firstName' => 'manager',
        'email' => 'm@m.com',
        'password' => 'manager!1M',
        'phone' => '+48000000000'
    ];

    private array $existentManager = [
        'lastName' => 'manager1',
        'firstName' => 'manager1',
        'email' => 'manager@manager.com',
        'password' => 'manager!1M',
        'phone' => '+48000000000'
    ];

    public function testCreate_validData(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $response = $this->post(
            uri: '/api/manager/create',
            data: $this->nonexistentManager,
            accessToken: $adminAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertJson($response->getContent());
        $this->assertSame($this->nonexistentManager['email'], json_decode($response->getContent(), true)['email']);
    }


    public function testCreate_invalidData(): void
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $response = $this->post(
            uri: '/api/manager/create',
            data: $this->existentManager,
            accessToken: $adminAccessToken,
        );
        $this->assertSame(409, $response->getStatusCode());
        $this->assertIsString($response->getContent());
        $this->assertSame("User manager@manager.com already exists", $response->getContent());
    }

    public function testInfo_validData(): void
    {
        $managerAccessToken = $this->login($this->existentManager['email'], $this->existentManager['password']);

        $response = $this->get(
            uri: '/api/manager/info',
            accessToken: $managerAccessToken
        );
        var_dump($response->getContent());
        $this->assertSame(200, $response->getStatusCode());
        $this->assertJson($response->getContent());
        $this->assertSame($this->existentManager['email'], json_decode($response->getContent(), true)['email']);
    }

    public function testInfo_invalidData(): void
    {
        $response = $this->get(
            uri: '/api/manager/info',
        );
        var_dump($response->getContent());
        $this->assertSame(401, $response->getStatusCode());
    }
}