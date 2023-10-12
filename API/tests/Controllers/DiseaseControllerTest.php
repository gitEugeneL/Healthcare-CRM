<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;
use Symfony\Component\HttpFoundation\Response;

class DiseaseControllerTest extends TestCase
{
    private array $disease = [
        'name' => 'influenza-19'
    ];

    private function createDisease(): Response
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        return $this->post(
            uri: '/api/disease/create',
            accessToken: $managerAccessToken,
            data: $this->disease
        );
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $createDiseaseResponse = $this->createDisease();

        $this->assertSame(201, $createDiseaseResponse->getStatusCode());
        $this->assertSame($this->disease['name'], $this->decodeResponse($createDiseaseResponse)['name']);
    }

    public function testCreate_withExistentDisease_returnsAlreadyExist(): void
    {
        for ($i = 0; $i < 2; $i++)
            $createDiseaseResponse = $this->createDisease();

        $this->assertSame(409, $createDiseaseResponse->getStatusCode());
    }

    public function testDelete_withValidId_returnsDeleted(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $disease = json_decode($this->createDisease()->getContent(), true);

        $response = $this->delete(
            uri: "/api/disease/delete/{$disease['id']}",
            accessToken: $managerAccessToken,
        );

        $this->assertSame(204, $response->getStatusCode());
    }

    public function testDelete_withInvalidId_returnsNotFound(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $response = $this->delete(
            uri: '/api/disease/delete/777',
            accessToken: $managerAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
    }

    public function testAddDoctor_withValidDoctorAndDiseaseId_returnsAdded(): void
    {
        $this->createDoctor();
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);
        $disease = json_decode($this->createDisease()->getContent(), true);

        $response = $this->patch(
            uri: "/api/disease/add-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );

        $this->assertSame(201, $response->getStatusCode());
    }

    public function testAddDoctor_withInvalidDoctorAndValidDiseaseId_returnsUnauthorized(): void
    {
        $disease = json_decode($this->createDisease()->getContent(), true);

        $response = $this->patch(
            uri: "/api/disease/add-doctor/{$disease['id']}",
            accessToken: 'doctor invalid token'
        );

        $this->assertSame(401, $response->getStatusCode());
    }

    public function testAddDoctor_witValidDoctorAndInvalidDiseaseId_returnsNotFound(): void
    {
        $this->createDoctor();
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);

        $response = $this->patch(
            uri: '/api/disease/add-doctor/777',
            accessToken: $doctorAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
    }

    public function testRemoveDoctor_withValidDoctorAndDisease_returnsRemoved(): void
    {
        $this->createDoctor();
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);
        $disease = json_decode($this->createDisease()->getContent(), true);

        $this->patch(
            uri: "/api/disease/add-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );
        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );

        $this->assertSame(201, $response->getStatusCode());
    }

    public function testRemoveDoctor_withValidDoctorAndInvalidDisease_returnsNotFound(): void
    {
        $this->createDoctor();
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);

        $response = $this->patch(
            uri: '/api/disease/remove-doctor/777',
            accessToken: $doctorAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
    }

    public function testRemoveDoctor_withInvalidDoctorAndValidDisease_returnsUnauthorized(): void
    {
        $disease = json_decode($this->createDisease()->getContent(), true);

        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$disease['id']}",
            accessToken: 'doctor invalid token'
        );
        $this->assertSame(401, $response->getStatusCode());
    }

    public function testRemoveDoctor_doctorNotHaveDisease_returnsNotFound(): void
    {
        $this->createDoctor();
        $doctorAccessToken = $this->login($this->doctor['email'], $this->doctor['password']);
        $disease = json_decode($this->createDisease()->getContent(), true);

        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Doctor doesn't have this disease", $response->getContent());
    }
}