<?php

namespace App\Tests\DtoValidator\Doctor;

use App\Constant\ValidationConstants;
use App\Dto\Doctor\UpdateDoctorDto;
use App\Tests\DtoTestCase;

class UpdateDoctorDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            ['Some text text', 'Warszawa Medical University'],
            [null, 'Medical University'],
            ['Jack', 'Cooper', '+48888555888', 'Some text text', 'Warszawa Medical University'],
            [null, null, '256698789', null, 'Medical University']
        ];
    }

    public function incorrectData(): array
    {
        return [
            [
                [
                    'phone' => ['365-987-621', ValidationConstants::INVALID_PHONE_NUMBER],
                    'description' => ['qwe', ValidationConstants::SHORT_VALUE_10],
                    'education' => ['qwe', ValidationConstants::SHORT_VALUE_10]
                ]
            ],
            [
                [
                    'phone' => ['+ASD365987', ValidationConstants::INVALID_PHONE_NUMBER],
                ]
            ],
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testUpdateDoctorDto_withCorrectData(
        string $firstName = null,
        string $lastName = null,
        string $phone = null,
        string $description = null,
        string $education = null
    ): void
    {
        $dto = new UpdateDoctorDto();
        if ($firstName)
            $dto->setFirstName($firstName);
        if ($lastName)
            $dto->setLastName($lastName);
        if ($phone)
            $dto->setPhone($phone);
        if ($description)
            $dto->setDescription($description);
        if ($education)
        $dto->setEducation($education);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectData
     */
    public function testUpdateDoctorDto_withIncorrectData(array $data): void
    {
        $dto = new UpdateDoctorDto();
        if (isset($data['firstName']))
            $dto->setFirstName($data['firstName'][0]);
        if (isset($data['lastName']))
            $dto->setLastName($data['lastName'][0]);
        if (isset($data['phone']))
            $dto->setPhone($data['phone'][0]);
        if (isset($data['description']))
            $dto->setDescription($data['description'][0]);
        if (isset($data['education']))
            $dto->setEducation($data['education'][0]);

        $violations = $this->validator->validate($dto);
        $result = $this->inspect($violations);

        foreach ($data as $k => $v) {
            if (!empty($v[1]))
                $this->assertSame($result[$k], $v[1]);
            else
                $this->assertArrayNotHasKey($k, $result);
        }
    }
}