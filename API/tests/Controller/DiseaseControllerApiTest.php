<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;
use Symfony\Component\HttpFoundation\Response;

class DiseaseControllerApiTest extends ApiTestCase
{
    private array $disease = [
        'name' => 'Influenza-19'
    ];

    private function createDisease(): Response
    {
        $managerAccessToken = $this->accessToken('manager');

        return $this->request(
            method: 'POST',
            uri: '/api/diseases',
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
        $managerAccessToken = $this->accessToken('manager');

        $responseData = $this->decodeResponse($this->createDisease());

        $response = $this->request(
            method: 'DELETE',
            uri: "/api/diseases/{$responseData['id']}",
            accessToken: $managerAccessToken,
        );

        $this->assertSame(204, $response->getStatusCode());
    }

    public function testDelete_withInvalidId_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        $response = $this->request(
            method: 'DELETE',
            uri: '/api/diseases/777',
            accessToken: $managerAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
    }

    public function testAddDoctor_withValidDoctorAndDiseaseId_returnsAdded(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $disease = $this->decodeResponse($this->createDisease());
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/diseases/add-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame('"Doctor successfully added"', $response->getContent());
    }

    public function testAddDoctor_withInvalidDoctorAndValidDiseaseId_returnsUnauthorized(): void
    {
        $disease = $this->decodeResponse($this->createDisease());

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/diseases/add-doctor/{$disease['id']}",
            accessToken: 'doctor invalid token'
        );

        $this->assertSame(401, $response->getStatusCode());
    }

    public function testAddDoctor_witValidDoctorAndInvalidDiseaseId_returnsNotFound(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/diseases/add-doctor/777',
            accessToken: $doctorAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
    }

    public function testRemoveDoctor_withValidDoctorAndDisease_returnsRemoved(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $disease = $this->decodeResponse($this->createDisease());

        $this->request(
            method: 'PATCH',
            uri: "/api/diseases/add-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/diseases/remove-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );

        $this->assertSame(201, $response->getStatusCode());
    }

    public function testRemoveDoctor_withValidDoctorAndInvalidDisease_returnsNotFound(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $response = $this->request(
            method: 'PATCH',
            uri: '/api/diseases/remove-doctor/777',
            accessToken: $doctorAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
    }

    public function testRemoveDoctor_withInvalidDoctorAndValidDisease_returnsUnauthorized(): void
    {
        $disease = $this->decodeResponse($this->createDisease());

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/diseases/remove-doctor/{$disease['id']}",
            accessToken: 'doctor invalid token'
        );
        $this->assertSame(401, $response->getStatusCode());
    }

    public function testRemoveDoctor_doctorNotHaveDisease_returnsNotFound(): void
    {
        $doctorAccessToken = $this->accessToken('doctor');

        $disease = $this->decodeResponse($this->createDisease());
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/diseases/remove-doctor/{$disease['id']}",
            accessToken: $doctorAccessToken
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Doctor doesn't have this disease", $response->getContent());
    }
}