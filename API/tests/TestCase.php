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
}