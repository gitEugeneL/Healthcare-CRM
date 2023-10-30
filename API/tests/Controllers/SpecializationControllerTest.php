<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;
use phpDocumentor\Reflection\DocBlock\Tags\Method;
use Symfony\Component\HttpFoundation\Response;

class SpecializationControllerTest extends TestCase
{
    private array $specialization = ['name' => 'test-specialization'];

    private function createSpecialization(string $managerAccessToken): Response
    {

        return $this->request(
            method: 'POST',
            uri: '/api/specialization/create',
            accessToken: $managerAccessToken,
            data: $this->specialization
        );
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $response = $this->createSpecialization($this->accessToken('manager'));

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($this->specialization['name'], $this->decodeResponse($response)['name']);
    }

    public function testCreate_withInvalidData_returnsAlreadyExists(): void
    {
        for ($i = 0; $i < 2; $i++)
            $response = $this->createSpecialization($this->accessToken('manager'));

        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame("Specialization {$this->specialization['name']} already exists", $response->getContent());
    }

    public function testShow_withValidUser_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');

        for ($i = 0; $i < 5; $i++) {
            $this->specialization = ['name' => "specialization{$i}"];
            $this->createSpecialization($managerAccessToken);
        }
        $response = $this->request(
            method: 'GET',
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
        $managerAccessToken = $this->accessToken('manager');

        $this->request(
            method: 'POST',
            uri: '/api/specialization/create',
            accessToken: $managerAccessToken,
            data: $this->specialization,
        );
        $this->specialization['description'] = 'some text';
        $response = $this->request(
            method: 'PATCH',
            uri: "/api/specialization/update/{$this->specialization['name']}",
            accessToken: $managerAccessToken,
            data: $this->specialization
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->specialization['name'], $responseData['name']);
        $this->assertSame($this->specialization['description'], $responseData['description']);
    }

    public function testUpdate_withInvalidData_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $this->specialization['name'] = 'specialization1';
        $this->specialization['description'] = 'new test description';

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/specialization/update/{$this->specialization['name']}",
            accessToken: $managerAccessToken,
            data: $this->specialization
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame("Specialization: {$this->specialization['name']} doesn't exist", $response->getContent());
    }

    public function testDelete_witInvalidData_returnsDeleted(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $this->createSpecialization($managerAccessToken);

        $response = $this->request(
            method: 'DELETE',
            uri: "/api/specialization/delete/{$this->specialization['name']}",
            accessToken: $managerAccessToken
        );

        $this->assertSame(204, $response->getStatusCode());
    }

    public function testIncludeDoctor_withValidData_returnsUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));

        for ($i = 1; $i <= 5; $i++) {
            $this->user['doctor']['email'] = "doctor{$i}@doctor.com";
            $this->createUser('doctor', $managerAccessToken);

            $this->request(
                method: 'PATCH',
                uri: '/api/specialization/include-doctor',
                accessToken: $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $specialization['name']
                ],
            );
        }
        $response = $this->request(
            method: 'GET',
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
        $managerAccessToken = $this->accessToken('manager');
        $this->createSpecialization($managerAccessToken);


        for ($i = 1; $i <= 5; $i++) {
            $this->user['doctor']['email'] = "doctor{$i}@doctor.com";
            $this->createUser('doctor', $managerAccessToken);

            $this->request(
                method: 'PATCH',
                uri: '/api/specialization/include-doctor',
                accessToken: $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $this->specialization['name']
                ],
            );
            $this->request(
                method: 'PATCH',
                uri: '/api/specialization/exclude-doctor',
                accessToken: $managerAccessToken,
                data: [
                    'doctorId' => $i,
                    'specializationName' => $this->specialization['name']
                ],
            );
        }
        $response = $this->request(
            method: 'GET',
            uri: "/api/doctor/show-by-specialization/{$this->specialization['name']}",
            accessToken: $managerAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(0, $responseData['items']);
    }
}