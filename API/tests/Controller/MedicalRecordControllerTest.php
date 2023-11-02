<?php

namespace App\Tests\Controller;

use App\Tests\TestCase;
use DateTime;
use Symfony\Component\HttpFoundation\Response;

class MedicalRecordControllerTest extends TestCase
{
    private function createSpecialization(string $managerAccessToken): Response
    {
        $data = ['name' => 'specialization1'];
        return $this->request(
            method: 'POST',
            uri: '/api/specialization/create',
            accessToken: $managerAccessToken,
            data: $data,
        );
    }

    private function includeDoctor(string $managerAccessToken, string $specializationName, int $doctorId): void
    {
        $this->request(
            method: 'PATCH',
            uri: '/api/specialization/include-doctor',
            accessToken: $managerAccessToken,
            data: [
                'doctorId' => $doctorId,
                'specializationName' => $specializationName
            ],
        );
    }

    private function createAppointment(string $patientAccessToken, int $doctorId, int $i = 1): Response
    {
        $date = (new DateTime())->modify("+{$i} day");
        $startTime = '12:00';

        if ($date->format('N') == 6 || $date->format('N') == 7) {
            $date->modify('+2 day');
            $startTime = '13:00';
        }

        $data = [
            'doctorId' => $doctorId,
            'date' => $date->format('Y-m-d'),
            'startTime' => $startTime
        ];
        return $this->request(
            method: 'POST',
            uri: '/api/appointment/create',
            accessToken: $patientAccessToken,
            data: $data
        );
    }

    private function createMedicalRecord(string $doctorAccessToken, string $patientEmail, int $specializationId, int $appointmentId = 1): Response
    {
        $medicalRecordData = [
            'title' => 'title1',
            'description' => 'description1',
            'patientEmail' => $patientEmail,
            'specializationId' => $specializationId,
            'appointmentId' => $appointmentId
        ];
        return $this->request(
            method: 'POST',
            uri: '/api/medical-record',
            accessToken: $doctorAccessToken,
            data: $medicalRecordData
        );
    }

    public function testCreate_withValidData_returnsCreated(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $appointment = $this->decodeResponse($this->createAppointment($patientAccessToken, 1));
        // create a medical record
        $response = $this->createMedicalRecord(
            $doctorAccessToken, $this->user['patient']['email'], $specialization['id'], $appointment['id']);

        $responseData = $this->decodeResponse($response);
        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($appointment['id'], $responseData['appointmentId']);
        $this->assertSame($this->user['patient']['email'], $responseData['patient']['email']);
        $this->assertSame(1, $responseData['doctor']['id']);
    }

    public function testCreate_withExistingRecord_returnsAlreadyExist(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $appointment = $this->decodeResponse($this->createAppointment($patientAccessToken, 1));
        // create a medical record
        for ($i = 0; $i < 2; $i++)
            $response = $this->createMedicalRecord(
                $doctorAccessToken, $this->user['patient']['email'], $specialization['id'], $appointment['id']);

        $this->assertSame(409, $response->getStatusCode());
        $this->assertSame('Appointment already has a medical record', $response->getContent());
    }

    public function testCreate_doctorWithoutSpecialization_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        // create an appointment
        $appointment = $this->decodeResponse($this->createAppointment($patientAccessToken, 1));
        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // create a medical record
        $response = $this->createMedicalRecord(
            $doctorAccessToken, $this->user['patient']['email'], $specialization['id'], $appointment['id']);

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Doctor does not have this specialization', $response->getContent());
    }

    public function testShowForDoctor_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);

        for ($i = 1; $i <= 20; $i++) {
            // create an appointment
            $this->createAppointment($patientAccessToken, 1, $i);
            // create a medical record
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id'], $i);
        }

        // show medical records
        $response = $this->request(
            method: 'GET',
            uri: '/api/medical-record/for-doctor/1?page=1',
            accessToken: $doctorAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(10, $responseData['items']);
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(2, $responseData['totalPages']);
        foreach ($responseData['items'] as $record) {
            $this->assertSame($this->user['doctor']['email'], $record['doctor']['email']);
        }
    }

    public function testShowForPatient_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);

        for ($i = 1; $i <= 5; $i++) {
            // create an appointment
            $this->createAppointment($patientAccessToken, 1, $i);
            // create a medical record
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id'], $i);
        }

        $response = $this->request(
            method: 'GET',
            uri: '/api/medical-record/for-patient?page=1',
            accessToken: $patientAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertCount(5, $responseData['items']);
        $this->assertSame(1, $responseData['currentPage']);
        $this->assertSame(1, $responseData['totalPages']);
        foreach ($responseData['items'] as $record) {
            $this->assertSame($this->user['patient']['email'], $record['patient']['email']);
        }
    }

    public function testShowOneForDoctor_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $doctorAccessToken = $this->accessToken('doctor');
        $patientAccessToken = $this->accessToken('patient');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $this->createAppointment($patientAccessToken, 1);
        // create a medical record
        $record = $this->decodeResponse(
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id']));

        $response = $this->request(
            method: 'GET',
            uri: "/api/medical-record/{$record['id']}/for-doctor",
            accessToken: $doctorAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['patient']['email'], $responseData['patient']['email']);
        $this->assertSame($this->user['doctor']['email'], $responseData['doctor']['email']);
        $this->assertSame($record['id'], $responseData['appointmentId']);
    }

    public function testShowOneForPatient_validRequest_returnsOk(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $doctorAccessToken = $this->accessToken('doctor');
        $patientAccessToken = $this->accessToken('patient');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $this->createAppointment($patientAccessToken, 1);
        // create a medical record
        $record = $this->decodeResponse(
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id']));

        $response = $this->request(
            method: 'GET',
            uri: "/api/medical-record/{$record['id']}/for-patient",
            accessToken: $patientAccessToken
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['patient']['email'], $responseData['patient']['email']);
        $this->assertSame($this->user['doctor']['email'], $responseData['doctor']['email']);
        $this->assertSame($record['id'], $responseData['id']);
    }

    public function testUpdate_validData_returnsUpdated(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $doctorAccessToken = $this->accessToken('doctor');
        $patientAccessToken = $this->accessToken('patient');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $this->createAppointment($patientAccessToken, 1);
        // create a medical record
        $record = $this->decodeResponse(
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id']));

        $data = [
            'title' => 'new title',
            'description' => 'new description',
            'doctorNote' => 'text'
        ];

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/medical-record/{$record['id']}",
            accessToken: $doctorAccessToken,
            data: $data
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(201, $response->getStatusCode());
        $this->assertSame($data['title'], $responseData['title']);
        $this->assertSame($data['description'], $responseData['description']);
        $this->assertSame($data['doctorNote'], $responseData['doctorNote']);
        $this->assertSame($record['id'], $responseData['id']);
    }

    public function testUpdate_WithoutData_returnsNotFound(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $doctorAccessToken = $this->accessToken('doctor');
        $patientAccessToken = $this->accessToken('patient');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $this->createAppointment($patientAccessToken, 1);
        // create a medical record
        $record = $this->decodeResponse(
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id']));

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/medical-record/{$record['id']}",
            accessToken: $doctorAccessToken,
        );

        $this->assertSame(404, $response->getStatusCode());
        $this->assertSame('Nothing to change', $response->getContent());
    }

    public function testUpdate_doctorWithoutAccess_returnsAccessDenied(): void
    {
        $managerAccessToken = $this->accessToken('manager');
        $patientAccessToken = $this->accessToken('patient');
        $doctorAccessToken = $this->accessToken('doctor');

        $this->user['doctor']['email'] = 'new-doctor@t.com';
        $this->createUser('doctor', $managerAccessToken);
        $doctorAccessToken1 = $this->accessToken('doctor');

        // create a specialization
        $specialization = $this->decodeResponse($this->createSpecialization($managerAccessToken));
        // include the doctor
        $this->includeDoctor($managerAccessToken, $specialization['name'], 1);
        // create an appointment
        $this->createAppointment($patientAccessToken, 1);
        // create a medical record
        $record = $this->decodeResponse(
            $this->createMedicalRecord($doctorAccessToken, $this->user['patient']['email'], $specialization['id']));

        $response = $this->request(
            method: 'PATCH',
            uri: "/api/medical-record/{$record['id']}",
            accessToken: $doctorAccessToken1,
            data: [
                'title' => 'new title',
                'description' => 'new description',
                'doctorNote' => 'text'
            ]
        );

        $this->assertSame(403, $response->getStatusCode());
        $this->assertSame("Doctor doesn't have access to update this medical record", $response->getContent());
    }
}
