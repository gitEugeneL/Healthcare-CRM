<?php

namespace App\Tests;

class SpecializationControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $specialization = ['name' => '   tEstSpEcializations '];
        $managerAccessToken = $this->createAndLoginManager();
        $response = $this->post(
            uri: '/api/specialization/create',
            data: $specialization,
            accessToken: $managerAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame(
            strtolower(trim($specialization['name'])), json_decode($response->getContent(), true)['name']);
    }

    public function testCreate_withInvalidData_returnsAlreadyExists(): void
    {
        $specialization = ['name' => 'specialization1'];
        $managerAccessToken = $this->createAndLoginManager();
        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/specialization/create',
                data: $specialization,
                accessToken: $managerAccessToken
            );
        }
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("Specialization {$specialization['name']} already exists", $response->getContent());
    }
}