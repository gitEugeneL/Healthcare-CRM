<?php

namespace App\Tests\Auth;

use App\Tests\AbstractApiTest;

class AuthApiTest extends AbstractApiTest
{
    public function testLogin_validData_returnsOkAndToken(): void
    {
        $response = $this->post(
            uri: '/api/token/login',
            data: [
                'username' => AuthData::getTestUser()['username'],
                'password' => AuthData::getTestUser()['password']
            ]
        );
        $responseData = json_decode($response->getContent(), true);
        AuthData::setAccessToken($responseData['token']);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertArrayHasKey('token', $responseData, "Response doesn't contain the token");
    }

    public function testLogin_InvalidData_returnsUnauthorized(): void
    {
        $response = $this->post('/api/token/login', ['username' => 't@t.t', 'password' => 'ttt']);
        $responseData = json_decode($response->getContent(), true);

        $this->assertSame(401, $response->getStatusCode());
        $this->assertArrayHasKey('message', $responseData);
    }
}