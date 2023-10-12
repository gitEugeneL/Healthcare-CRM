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
        'email' => 'd@d.com',
        'password' => 'doctor!1',
        'lastName' => 'doctor',
        'firstName' => 'doctor'
    ];

    protected array $patient = [
        'email' => 'p@p.com',
        'password' => 'patient1!A',
        'lastName' => 'patient',
        'firstName' => 'patient'
    ];

    protected function setUp(): void
    {
        $this->client = static::createClient();
    }

    private function setAccessToken(string $accessToken): void
    {
        $this->headers['HTTP_AUTHORIZATION'] = 'Bearer ' . $accessToken;
    }

    private function request(string $method, string $uri, string $accessToken = null, array $data = []): Response
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

    protected function post(string $uri, string $accessToken = null, array $data = []): Response
    {
        return $this->request('POST', $uri, $accessToken, $data);
    }

    protected function patch(string $uri, string $accessToken = null, $data = []): Response
    {
        return $this->request('PATCH', $uri, $accessToken, $data);
    }

    protected function put(string $uri, string $accessToken = null, $data = []): Response
    {
        return $this->request('PUT', $uri, $accessToken, $data);
    }

    protected function get(string $uri, string $accessToken = null): Response
    {
        return $this->request('GET', $uri, $accessToken);
    }

    protected function delete(string $uri, string $accessToken = null): Response
    {
        return $this->request('DELETE', $uri, $accessToken);
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

    protected function decodeResponse(Response $response): array
    {
        return json_decode($response->getContent(), true);
    }

    protected function createManager(): Response
    {
        $adminAccessToken = $this->login($this->admin['username'], $this->admin['password']);

        return $this->post(
            uri: '/api/manager/create',
            accessToken: $adminAccessToken,
            data: $this->manager
        );
    }

    protected function createDoctor(): Response
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        return $this->post(
            uri: '/api/doctor/create',
            accessToken: $managerAccessToken,
            data: $this->doctor
        );
    }

    protected function createPatient(): Response
    {
        return $this->post(
            uri: '/api/patient/create',
            data: $this->patient
        );
    }
}