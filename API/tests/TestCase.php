<?php

namespace App\Tests;

use Symfony\Bundle\FrameworkBundle\KernelBrowser;
use Symfony\Bundle\FrameworkBundle\Test\WebTestCase;
use Symfony\Component\HttpFoundation\Response;

abstract class TestCase extends WebTestCase
{
    private array $headers = [
        'HTTP_ACCEPT' => 'application/json',
        'CONTENT_TYPE' => 'application/json',
    ];

    protected KernelBrowser $client;

    protected array $admin = [
        'username' => 'admin@admin.com',
        'password' => 'admin!1A'
    ];

    protected array $manager = [
        'lastName' => 'manager',
        'firstName' => 'manager',
        'email' => 'm@m.com',
        'password' => 'manager!1M',
    ];

    protected array $doctor = [
        'email' => 'doctor@doctor.com',
        'password' => 'doctor!1',
        'lastName' => 'doctor1',
        'firstName' => 'doctor1'
    ];

    protected function setUp(): void
    {
        $this->client = static::createClient();
    }

    private function setAccessToken(string $accessToken): void
    {
        $this->headers['HTTP_AUTHORIZATION'] = 'Bearer ' . $accessToken;
    }

    protected function post(string $uri, array $data = [], $accessToken = null): Response
    {
        if ($accessToken)
            $this->setAccessToken($accessToken);

        $this->client->request(
          method: 'POST',
          uri: $uri,
          server: $this->headers,
          content: json_encode($data)
        );
        return $this->client->getResponse();
    }

    protected function patch(string $uri, array $data = [], $accessToken = null): Response
    {
        if ($accessToken)
            $this->setAccessToken($accessToken);

        $this->client->request(
            method: 'PATCH',
            uri: $uri,
            server: $this->headers,
            content: json_encode($data)
        );
        return $this->client->getResponse();
    }

    protected function get(string $uri, $accessToken = null): Response
    {
        if ($accessToken)
            $this->setAccessToken($accessToken);

        $this->client->request(
            method: 'GET',
            uri: $uri,
            server: $this->headers
        );
        return $this->client->getResponse();
    }

    protected function login(string $username, string $password): string
    {
        $response = $this->post(
            uri: '/api/token/login',
            data: [
                'username' => $username,
                'password' => $password
            ]
        );
        return json_decode($response->getContent(), true)['token'];
    }

    protected function createAndLoginManager(): string
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);
        $this->post(
            uri: '/api/manager/create',
            data: $this->manager,
            accessToken: $adminAccessToken
        );
        return $this->login($this->manager['email'], $this->manager['password']);
    }

    protected function createAndLoginDoctor(): string
    {
        $managerAccessToken = $this->createAndLoginManager();
        $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        return $this->login($this->doctor['email'], $this->doctor['password']);
    }
}