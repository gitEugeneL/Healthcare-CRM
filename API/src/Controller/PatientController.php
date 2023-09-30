<?php

namespace App\Controller;

use App\Dto\Patient\CreatePatientDto;
use App\Exception\DtoRequestException;
use App\Service\PatientService;
use App\Validator\DtoValidator;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/patient')]
class PatientController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoValidator $dtoValidator,
        private readonly PatientService $patientService
    ) {}

    /**
     * @throws DtoRequestException
     */
    #[IsGranted('PUBLIC_ACCESS')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreatePatientDto::class, 'json');
        $this->dtoValidator->validate($dto);
        $result = $this->patientService->create($dto);
        return $this->json($result, 201);
    }
}