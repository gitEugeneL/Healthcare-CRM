<?php

namespace App\Tests\Auth;

class AuthData
{
    private static string $accessToken;

    private static array $testUser = [
        'username' => 'test@test.com',
        'password' => 'test'
    ];

    public static function setAccessToken(string $accessToken): void
    {
        self::$accessToken = $accessToken;
    }

    public static function getAccessToken(): string
    {
        return self::$accessToken;
    }

    public static function getTestUser(): array
    {
        return self::$testUser;
    }
}