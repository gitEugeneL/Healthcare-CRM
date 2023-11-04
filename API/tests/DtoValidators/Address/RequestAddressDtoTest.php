<?php

namespace App\Tests\DtoValidators\Address;

use App\Dto\Address\RequestAddressDto;
use App\Tests\DtoTestCase;


class RequestAddressDtoTest extends DtoTestCase
{
    public function validAddresses(): array
    {
        return [
            ['Warszawa', 'Marszałkowska', '05-999', 'Mazowieckie', '35/38'],
            ['Kraków', 'Augustynka-wichury', '05-999', 'Małopolskie', '31', '5'],
            ['Opole', 'Plutonu AK "Torpedy"', '05-350', 'Opolskie', '75F', '85F'],
        ];
    }

    public function invalidAddresses(): array
    {
        return
            [
                [
                    [
                        'city' => ['', 'This value should not be blank.'],
                        'street' => ['', 'This value should not be blank.'],
                        'postalCode' => ['', 'This value should not be blank.'],
                        'province' => ['', 'This value should not be blank.'],
                        'house' => ['', 'This value should not be blank.'],
                    ],
                ],
                [
                    [
                        'city' => ['Warszawa', null],
                        'street' => ['Marszałkowska', null],
                        'postalCode' => ['1236587ASD', 'Valid postal code format: 00-000'],
                        'province' => ['Mazowieckie', null],
                        'house' => ['999999', 'This value is too long. It should have 5 characters or less.'],
                        'apartment' => ['999999', 'This value is too long. It should have 5 characters or less.']
                    ],
                ],
                [
                    [
                        'city' => ['Kraków', null],
                        'street' => ['Marszałkowska', null],
                        'postalCode' => ['AA-DDD', 'Valid postal code format: 00-000'],
                        'province' => ['Małopolskie', null],
                        'house' => ['25/D', null],
                        'apartment' => ['5A', null]
                    ],
                ]
            ];
    }

    /**
     * @dataProvider validAddresses
     */
    public function testValidDto(
        string $city, string $street, string $postalCode, string $province, string $house, string$apartment = null
    ): void
    {
        $dto = new RequestAddressDto();
        $dto->setCity($city);
        $dto->setStreet($street);
        $dto->setPostalCode($postalCode);
        $dto->setProvince($province);
        $dto->setHouse($house);
        if ($apartment)
            $dto->setApartment($apartment);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider invalidAddresses
     */
    public function testInvalidDto(array $data): void
    {
        $dto = new RequestAddressDto();
        $dto->setCity($data['city'][0]);
        $dto->setStreet($data['street'][0]);
        $dto->setPostalCode($data['postalCode'][0]);
        $dto->setProvince($data['province'][0]);
        $dto->setHouse($data['house'][0]);
        if (isset($data['apartment']))
            $dto->setApartment($data['apartment'][0]);

        $violations = $this->validator->validate($dto);
        $result = $this->inspect($violations);

        foreach ($data as $k => $v) {
            if (!empty($v[1]))
                // If the error message is not empty, assert that it matches the expected message
                $this->assertSame($result[$k], $v[1]);
            else
                // If the error message is empty, assert that the field has no errors
                $this->assertArrayNotHasKey($k, $result);
        }
    }
}