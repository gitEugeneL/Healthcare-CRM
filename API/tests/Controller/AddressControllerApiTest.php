<?php

namespace App\Tests\Controller;

use App\Tests\ApiTestCase;

class AddressControllerApiTest extends ApiTestCase
{
    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $patientAccessToken = $this->accessToken('patient');

        $updateData = [
            'city' => 'Warszawa',
            'street' => 'Marszałkowska',
            'postalCode' => '02-013',
            'province' => 'Mazowieckie',
            'house' => '3/5',
            'apartment' => '3B'
        ];

        $response = $this->request(
            method: 'PUT',
            uri: '/api/addresses',
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