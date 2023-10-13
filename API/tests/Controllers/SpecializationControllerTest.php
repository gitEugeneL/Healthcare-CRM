<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;
use Symfony\Component\HttpFoundation\Response;

class SpecializationControllerTest extends TestCase
{
    private array $specialization = ['name' => 'specialization1'];

    private function createSpecialization(): Response
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        return $this->post(
            uri: '/api/specialization/create',
            accessToken: $managerAccessToken,
            data: $this->specialization
        );
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $response = $this->createSpecialization();

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->specialization['name'], $this->decodeResponse($response)['name']);
    }

    public function testCreate_withInvalidData_returnsAlreadyExists(): void
    {
        for ($i = 0; $i < 2; $i++) {
            $response = $this->createSpecialization();
        }
        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("Specialization {$this->specialization['name']} already exists", $response->getContent());
    }

    public function testShow_withValidUser_returnsOk(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        for ($i = 0; $i < 5; $i++) {
            $this->specialization = ['name' => "specialization{$i}"];
            $this->createSpecialization();
        }
        $response = $this->get(
            uri: '/api/specialization/show',
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame('specialization0', $responseData[0]['name']);
        $this->assertSame('specialization4', $responseData[4]['name']);
    }

    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $this->post(
            uri: '/api/specialization/create',
            accessToken: $managerAccessToken,
            data: $this->specialization,
        );
        $this->specialization['description'] = 'some text';
        $response = $this->patch(
            uri: "/api/specialization/update/{$this->specialization['name']}",
            accessToken: $managerAccessToken,
            data: $this->specialization
        );
        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->specialization['name'], $responseData['name']);
        $this->assertSame($this->specialization['description'], $responseData['description']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $this->specialization['name'] = 'new test name';
        $this->specialization['description'] = 'new test description';

        $response = $this->patch(
            uri: "/api/specialization/update/{$this->specialization['name']}",
            accessToken: $managerAccessToken,
            data: $this->specialization
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Specialization not found', $response->getContent());
    }

    public function testDelete_witInvalidData_returnsDeleted(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $this->createSpecialization();

        $response = $this->delete(
            uri: "/api/specialization/delete/{$this->specialization['name']}",
            accessToken: $managerAccessToken
        );

        $this->assertSame(204, $response->getStatusCode());
    }

    public function testIncludeDoctor_withValidData_returnsUpdated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);

        $specialization = $this->decodeResponse($this->createSpecialization());

        for ($i = 1; $i <= 5; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->createDoctor();

            $this->patch(
                uri: '/api/specialization/include-doctor',
                accessToken: $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ],
            );
        }
        $response = $this->get(
            uri: "/api/doctor/show-by-specialization/{$specialization['name']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(5, $responseData['items']);
        foreach ($responseData['items'] as $doctor) {
            $this->assertSame($this->specialization['name'], $doctor['specializations'][0]['name']);
        }
    }

    public function testExcludeDoctor_withValidData_returnsUpdated(): void
    {
        $this->createManager();
        $managerAccessToken = $this->login($this->manager['email'], $this->manager['password']);
        $this->createSpecialization();


        for ($i = 1; $i <= 5; $i++) {
            $this->doctor['email'] = "doctor{$i}@doctor.com";
            $this->createDoctor();

            $this->patch(
                uri: '/api/specialization/include-doctor',
                accessToken:  $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $this->specialization['name']
                ],
            );
            $this->patch(
                uri: '/api/specialization/exclude-doctor',
                accessToken:  $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $this->specialization['name']
                ],
            );
        }
        $response = $this->get(
            uri: "/api/doctor/show-by-specialization/{$this->specialization['name']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(0, $responseData['items']);
    }
}