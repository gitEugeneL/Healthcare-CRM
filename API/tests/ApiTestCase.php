<?php

namespace App\Tests;

use Symfony\Bundle\FrameworkBundle\KernelBrowser;
use Symfony\Bundle\FrameworkBundle\Test\WebTestCase;
use Symfony\Component\HttpFoundation\Response;

abstract class ApiTestCase extends WebTestCase
{
    private array $headers = [
        'HTTP_ACCEPT' => 'application/json',
        'CONTENT_TYPE' => 'application/json',
    ];

    protected KernelBrowser $client;
    protected array $user = [
        'admin' => [
            'email' => 'a@a.com',
            'password' => 'admin!1A',
            'lastName' => 'Admin',
            'firstName' => 'Admin',
        ],
        'manager' => [
            'email' => 'm@m.com',
            'password' => 'manager!1M',
            'lastName' => 'Manager',
            'firstName' => 'Manager'
        ],
        'doctor' => [
            'email' => 'd@d.com',
            'password' => 'doctor!1',
            'lastName' => 'Doctor',
            'firstName' => 'Doctor'
        ],
        'patient' => [
            'email' => 'p@p.com',
            'password' => 'patient1!A',
            'lastName' => 'Patient',
            'firstName' => 'Patient'
        ]
    ];

    protected function setUp(): void
    {
        $this->client = static::createClient();
    }

    private function setAccessToken(string $accessToken): void
    {
        $this->headers['HTTP_AUTHORIZATION'] = 'Bearer ' . $accessToken;
    }

    private function login(string $username, string $password): string
    {
        $response = $this->request(
            method: 'POST',
            uri: '/api/token/login',
            data: [
                'username' => $username,
                'password' => $password
            ]
        );
        return json_decode($response->getContent(), true)['token'];
    }

    protected function request(string $method, string $uri, string $accessToken = null, array $data = []): Response
    {
        if ($accessToken)
            $this->setAccessToken($accessToken);

        $this->client->request(
            method: $method,
            uri: $uri,
            server: $this->headers,
            content: json_encode($data)
        );
        return $this->client->getResponse();
    }

    protected function decodeResponse(Response $response): array
    {
        return json_decode($response->getContent(), true);
    }

    protected function createUser(string $userType, string $accessToken = null): Response|null
    {
        if ($userType === 'manager' || $userType === 'doctor' || $userType === 'patient') {
            return $this->request(
                method: 'POST',
                uri: "/api/{$userType}s",
                accessToken: $accessToken,
                data: $this->user[$userType]
            );
        }
        return null;
    }

    protected function accessToken(string $userType): string
    {
        if ($userType === 'admin' || $userType === 'manager' || $userType === 'doctor' || $userType === 'patient')
            return $this->login($this->user[$userType]['email'], $this->user[$userType]['password']);
        else
            return '';
    }
}