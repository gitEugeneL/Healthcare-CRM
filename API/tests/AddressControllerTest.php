<?php

namespace App\Tests;

class AddressControllerTest extends TestCase
{
    public function testUpdate_withValidData_returnsUpdated(): void
    {
        $updateData = [
            'city' => 'warszawa',
            'street' => 'marszałkowska',
            'postalCode' => '02-013',
            'province' => 'mazowieckie',
            'house' => '3/5',
            'apartment' => '3B'
        ];
        $patientAccessToken = $this->createAndLoginPatient();
        $response = $this->put(
            uri: '/api/address/update',
            accessToken: $patientAccessToken,
            data: $updateData
        );

        $responseData = json_decode($response->getContent(), true);
        $this->assertSame(200, $response->getStatusCode());
        $this->assertSame($updateData['city'], $responseData['address']['city']);
    }
}