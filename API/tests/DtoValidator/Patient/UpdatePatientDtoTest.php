<?php

namespace App\Tests\DtoValidator\Patient;

use App\Constant\ValidationConstants;
use App\Dto\Patient\UpdatePatientDto;
use App\Tests\DtoTestCase;

class UpdatePatientDtoTest extends DtoTestCase
{
    public function correctData(): array
    {
        return [
            ['45080165898', '1945-08-01'],
            ['98032565486', '1998-03-25', 'nfz']
        ];
    }

    public function incorrectData(): array
    {
        return [
            [
                [
                    'pesel' => ['some text', ValidationConstants::INVALID_PESEL],
                    'dateOfBirth' => ['1993-25-03', ValidationConstants::INVALID_DATE_OF_BIRTH]
                ]
            ],
            [
                [
                    'pesel' => ['98416', ValidationConstants::INVALID_LENGTH_11]
                ]
            ],
        ];
    }

    /**
     * @dataProvider correctData
     */
    public function testUpdatePatientDto_withCorrectData(
        string $pesel = null, string $dateOfBirth = null, string $insurance = null
    ): void
    {
        $dto = new UpdatePatientDto();
        if ($pesel)
            $dto->setPesel($pesel);
        if ($dateOfBirth)
            $dto->setDateOfBirth($dateOfBirth);
        if ($insurance)
            $dto->setInsurance($insurance);

        $violations = $this->validator->validate($dto);
        $this->assertCount(0, $violations);
    }

    /**
     * @dataProvider incorrectData
     */
    public function testUpdatePatientDto_withIncorrectData(array $data): void
    {
        $dto = new UpdatePatientDto();
        if (isset($data['pesel'][0]))
            $dto->setPesel($data['pesel'][0]);
        if (isset($data['dateOfBirth'][0]))
            $dto->setDateOfBirth($data['dateOfBirth'][0]);
        if (isset($data['insurance'][0]))
            $dto->setInsurance($data['insurance'][0]);

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