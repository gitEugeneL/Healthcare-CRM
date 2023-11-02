<?php

namespace App\Tests\Controller;

use App\Tests\TestCase;

class AddressControllerTest extends TestCase
{
    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $patientAccessToken = $this->accessToken('patient');

        $updateData = [
            'city' => 'warszawa',
            'street' => 'marszałkowska',
            'postalCode' => '02-013',
            'province' => 'mazowieckie',
            'house' => '3/5',
            'apartment' => '3B'
        ];

        $response = $this->request(
            method: 'PUT',
            uri: '/api/address/update',
            accessToken: $patientAccessToken,
            data: $updateData
        );
        $responseData = $this->decodeResponse($response);

        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($this->user['patient']['email'], $responseData['email']);
        $this->assertSame($updateData['city'], $responseData['address']['city']);
        $this->assertSame($updateData['street'], $responseData['address']['street']);
    }
}