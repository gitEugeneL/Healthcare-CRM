<?php

use App\Tests\TestCase;

class DiseaseControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $disease = ['name' => 'influenza'];
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->post(
            uri: '/api/disease/create',
            accessToken: $managerAccessToken,
            data: $disease
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($disease['name'], json_decode($response->getContent(), true)['name']);
    }

    public function testCreate_withExistentDisease_returnsAlreadyExist(): void
    {
        $disease = ['name' => 'influenza'];
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/disease/create',
                accessToken: $managerAccessToken,
                data: $disease
            );
        }
        $this->assertSame(409, $response->getStatusCode());
    }

    public function testDelete_withValidId_returnsDeleted(): void
    {
        $disease = ['name' => 'influenza'];
        $managerAccessToken = $this->createAndLoginManager();
        $createResponse = $this->post(
            uri: '/api/disease/create',
            accessToken: $managerAccessToken,
            data: $disease
        );
        $diseaseId = json_decode($createResponse->getContent(), true)['id'];
        $response = $this->delete(
            uri: "/api/disease/delete/{$diseaseId}",
            accessToken: $managerAccessToken,
        );
        $this->assertSame(204, $response->getStatusCode());
    }

    public function testDelete_withInvalidId_returnsNotFound(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->delete(
            uri: '/api/disease/delete/1',
            accessToken: $managerAccessToken,
        );
        $this->assertSame(404, $response->getStatusCode());
    }
}