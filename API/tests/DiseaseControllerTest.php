<?php

use App\Tests\TestCase;

class DiseaseControllerTest extends TestCase
{
    public function testCreate_withValidData_returnsCreated(): void
    {
        $disease = ['name' => 'test'];
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
        $response = $this->post(
            uri: '/api/disease/create',
            accessToken: $managerAccessToken,
            data: $disease
        );
        $this->assertSame(409, $response->getStatusCode());
    }

    public function testDelete_withValidId_returnsDeleted(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $diseaseId = 1;
        $response = $this->delete(
            uri: "/api/disease/delete/{$diseaseId}",
            accessToken: $managerAccessToken,
        );
        $this->assertSame(204, $response->getStatusCode());
    }

    public function testDelete_withInvalidId_returnsNotFound(): void
    {
        $managerAccessToken = $this->createAndLoginManager();
        $diseaseId = 99;
        $response = $this->delete(
            uri: "/api/disease/delete/{$diseaseId}",
            accessToken: $managerAccessToken,
        );
        $this->assertSame(404, $response->getStatusCode());
    }

    public function testAddDoctor_withValidDoctorAndDiseaseId_returnsAdded(): void
    {
        $diseaseId = 1;
        $doctorAccessToken = $this->createAndLoginDoctor();
        $response = $this->patch(
            uri: "/api/disease/add-doctor/{$diseaseId}",
            accessToken: $doctorAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
    }

    public function testAddDoctor_withInvalidDoctorAndValidDiseaseId_returnsUnauthorized(): void
    {
        $diseaseId = 1;
        $response = $this->patch(
            uri: "/api/disease/add-doctor/{$diseaseId}",
            accessToken: 'doctor invalid token'
        );
        $this->assertSame(401, $response->getStatusCode());
    }

    public function testAddDoctor_witValidDoctorAndInvalidDiseaseId_returnsNotFound(): void
    {
        $doctorAccessToken = $this->createAndLoginDoctor();
        $diseaseId = 99;
        $response = $this->patch(
            uri: "/api/disease/add-doctor/{$diseaseId}",
            accessToken: $doctorAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
    }

    public function testRemoveDoctor_withValidDoctorAndDisease_returnsRemoved():void
    {
        $doctorAccessToken = $this->createAndLoginDoctor();
        $diseaseId = 1;
        $this->patch(
            uri: "/api/disease/add-doctor/{$diseaseId}",
            accessToken: $doctorAccessToken
        );
        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$diseaseId}",
            accessToken: $doctorAccessToken
        );
        $this->assertSame(201, $response->getStatusCode());
    }

    public function testRemoveDoctor_withValidDoctorAndInvalidDisease_returnsNotFound(): void
    {
        $doctorAccessToken = $this->createAndLoginDoctor();
        $diseaseId = 99;
        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$diseaseId}",
            accessToken: $doctorAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
    }

    public function testRemoveDoctor_withInvalidDoctorAndValidDisease_returnsUnauthorized(): void
    {
        $diseaseId = 1;
        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$diseaseId}",
            accessToken: 'doctor invalid token'
        );
        $this->assertSame(401, $response->getStatusCode());
    }

    public function testRemoveDoctor_doctorNotHaveDisease_returnsNotFound(): void
    {
        $doctorAccessToken = $this->createAndLoginDoctor();
        $diseaseId = 1;
        $response = $this->patch(
            uri: "/api/disease/remove-doctor/{$diseaseId}",
            accessToken: $doctorAccessToken
        );
        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Doctor doesn't have this disease", $response->getContent());
    }
}