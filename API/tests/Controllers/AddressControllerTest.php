<?php

namespace App\Tests\Controllers;

use App\Tests\TestCase;

class AddressControllerTest extends TestCase
{
    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $patient = $this->decodeResponse($this->createPatient());
        $patientAccessToken = $this->login($this->patient['email'], $this->patient['password']);

        $updateData = [
            'city' => 'warszawa',
            'street' => 'marszałkowska',
            'postalCode' => '02-013',
            'province' => 'mazowieckie',
            'house' => '3/5',
            'apartment' => '3B'
        ];

        $response = $this->put(
            uri: '/api/address/update',
            accessToken: $patientAccessToken,
            data: $updateData
        );

        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($patient['email'], $responseData['email']);
        $this->assertSame($updateData['city'], $responseData['address']['city']);
        $this->assertSame($updateData['street'], $responseData['address']['street']);
    }
}