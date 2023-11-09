<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;
use Symfony\Component\HttpFoundation\Response;

class ManagerControllerApiTest extends ApiTestCase
{
    private function createNewManager(string $adminAccessToken): Response
    {
        $this->user['manager']['email'] = 'test@m.com';
        return $this->createUser('manager', $adminAccessToken);
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $response = $this->createNewManager($this->accessToken('admin'));
        $responseData = $this->decodeResponse($response);
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->user['manager']['email'], $responseData['email']);
    }

    public function testCreate_witExistentManager_returnsAlreadyExist(): void
    {
        $response = $this->createUser('manager', $this->accessToken('admin'));

        $this->assertSame(422, $response->getStatusCode());
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $updateData = [
            'firstName' => 'New first name',
            'lastName' => 'New last name',
            'phone' => '+48999888999',
            'position' => 'The best manager'
        ];

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/managers',
            accessToken: $managerAccessToken,
            data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['firstName'], $responseData['firstName']);
        $this->assertSame($updateData['lastName'], $responseData['lastName']);
        $this->assertSame($updateData['phone'], $responseData['phone']);
        $this->assertSame($updateData['position'], $responseData['position']);
        $this->assertSame($this->user['manager']['email'], $responseData['email']);
    }
}