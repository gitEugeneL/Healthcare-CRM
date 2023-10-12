<?php

namespace App\Tests\Authentication;

use App\Tests\TestCase;
use Symfony\Component\BrowserKit\Cookie;
use Symfony\Component\HttpFoundation\Response;

class AuthTest extends TestCase
{
    private function userLogin(string $username, string $password): Response
    {
        return $this->post(
            uri: '/api/token/login',
            data: [
                'username' => $username,
                'password' => $password
            ]
        );
    }

    public function testLogin_withValidUser_returnsOkAndAccessTokenAndRefreshToken(): void
    {
        $response = $this->userLogin($this->admin['username'], $this->admin['password']);

        $this->assertSame(200, $response->getStatusCode());
        // accessToken --------------------------------------------------------
        $responseData = json_decode($response->getContent(), true);
        $this->assertTrue(isset($responseData['token']));
        $this->assertNotEmpty($responseData['token']);
        // refreshToken -------------------------------------------------------
        $this->assertTrue($response->headers->has('Set-Cookie'));
        $refreshTokenCookie = $response->headers->getCookies()[0];
        $this->assertNotEmpty($refreshTokenCookie->getValue());
    }

    public function testLogin_withInvalidUser_returnsUnauthorized(): void
    {
        $response = $this->userLogin($this->admin['username'], '0000');

        $this->assertSame(401, $response->getStatusCode());
        $this->assertFalse($response->headers->has('Set-Cookie'));

        $responseData = json_decode($response->getContent(), true);
        $expectedJson = [
            'code' => 401,
            'message' => 'Invalid credentials.',
        ];
        $this->assertEquals($expectedJson, $responseData);
    }

    public function testRefresh_withValidUser_returnsOkAndSetNewRefreshToken(): void
    {
        $loginResponse = $this->userLogin($this->admin['username'], $this->admin['password']);

        $refreshToken = $loginResponse->headers->getCookies()[0]->getValue();
        // set refreshToken to cookie ----------------------------------------
        $cookie = new Cookie('refreshToken', $refreshToken);
        $this->client->getCookieJar()->set($cookie);
        // -------------------------------------------------------------------
        $refreshResponse = $this->post(
            uri: '/api/token/refresh'
        );
        $this->assertSame(200, $refreshResponse->getStatusCode());
        $newRefreshToken = $refreshResponse->headers->getCookies()[0]->getValue();
        $this->assertNotEmpty($newRefreshToken);
        $this->assertNotEquals($refreshToken, $newRefreshToken);
    }

    public function testRefresh_withVInvalidRefreshToken_returnsUnauthorized(): void
    {
        $refreshToken = 'invalidRefreshToken';
        // set refreshToken to cookie ----------------------------------------
        $cookie = new Cookie('refreshToken', $refreshToken);
        $this->client->getCookieJar()->set($cookie);
        // -------------------------------------------------------------------
        $refreshResponse = $this->post(
            uri: '/api/token/refresh'
        );
        $this->assertSame(401, $refreshResponse->getStatusCode());
        $this->assertFalse($refreshResponse->headers->has('Set-Cookie'));
    }

    public function testLogout_withValidRefreshToken_returnOkAndWithoutNewRefreshToken(): void
    {
        $loginResponse = $this->userLogin($this->admin['username'], $this->admin['password']);

        $refreshToken = $loginResponse->headers->getCookies()[0]->getValue();
        // set refreshToken to cookie ----------------------------------------
        $cookie = new Cookie('refreshToken', $refreshToken);
        $this->client->getCookieJar()->set($cookie);
        // -------------------------------------------------------------------

        $logoutResponse = $this->post(
            uri: '/api/token/invalidate'
        );
        $responseData = json_decode($logoutResponse->getContent(), true);
        $expectedJson = [
            'code' => 200,
            'message' => 'The supplied refresh_token has been invalidated.',
        ];
        $this->assertSame(200, $logoutResponse->getStatusCode());
        $this->assertEquals($expectedJson, $responseData);
        $this->assertEmpty($logoutResponse->headers->getCookies()[0]->getValue());
    }

    public function testLogout_withoutValidRefreshToken_returnsTokenAlreadyInvalid(): void
    {
        $refreshToken = 'invalidRefreshToken';
        // set refreshToken to cookie ----------------------------------------
        $cookie = new Cookie('refreshToken', $refreshToken);
        $this->client->getCookieJar()->set($cookie);
        // -------------------------------------------------------------------
        $logoutResponse = $this->post(
            uri: '/api/token/invalidate'
        );
        $responseData = json_decode($logoutResponse->getContent(), true);
        $expectedJson = [
            'code' => 200,
            'message' => 'The supplied refresh_token is already invalid.',
        ];
        $this->assertSame(200, $logoutResponse->getStatusCode());
        $this->assertEquals($expectedJson, $responseData);
        $this->assertEmpty($logoutResponse->headers->getCookies()[0]->getValue());
    }
}