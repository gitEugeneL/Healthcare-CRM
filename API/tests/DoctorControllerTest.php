<?php

namespace App\Tests;

class DoctorControllerTest extends TestCase
{
    private array $doctor = [
        'email' => 'doctor@doctor.com',
        'password' => 'doctor!1',
        'lastName' => 'doctor1',
        'firstName' => 'doctor1'
    ];

    public function testCreate_withValidData_returnsCreated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $response = $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
        $this->assertJson($response->getContent());
        $this->assertSame($this->doctor['email'], json_decode($response->getContent(), true)['email']);
    }

    public function testCreate_withExistentDoctor_returnsAlreadyExist(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        for ($i = 0; $i < 2; $i++) {
            $response = $this->post(
                uri: '/api/doctor/create',
                data: $this->doctor,
                accessToken: $managerAccessToken
            );
        }
        $this->assertSame(409, $response->getStatusCode());
        $this->assertIsString($response->getContent());
        $this->assertSame("User {$this->doctor['email']} already exists", $response->getContent());
    }

    public function testShow_validRequest_returnsOk(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        for ($i = 0; $i < 20; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->post(
                uri: '/api/doctor/create',
                data: $this->doctor,
                accessToken: $managerAccessToken
            );
        }
        $response = $this->get(
            uri: '/api/doctor/show?page=1',
            accessToken: $managerAccessToken
        );

        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
    }

    public function testShowOne_validId_returnsOk(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $this->post(
            uri: '/api/doctor/create',
            data: $this->doctor,
            accessToken: $managerAccessToken
        );
        $response = $this->get(
            uri: '/api/doctor/show/1',
            accessToken: $managerAccessToken
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame(1, $responseData['id']);
        $this->assertSame($this->doctor['email'], $responseData['email']);
    }

    public function testShowOne_invalidId_returnsNotFound(): void
    {
        $id = 1;
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $response = $this->get(
            uri: "/api/doctor/show/{$id}",
            accessToken: $managerAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
        $this->assertIsString($response->getContent());
        $this->assertSame("User id:{$id} not found", $response->getContent());
    }
}