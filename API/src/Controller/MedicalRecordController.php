<?php

namespace App\Controller;

use App\Dto\MedicalRecord\RequestMedicalRecordDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\MedicalRecordService;
use App\Utils\DtoInspector;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/medical-record')]
class MedicalRecordController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly MedicalRecordService $medicalRecordService
    ) {}

    /**
     * @throws ValidationException
     * @throws AlreadyExistException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('', methods: ['POST'])]
    public function create(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), RequestMedicalRecordDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->medicalRecordService->create($doctorIdentifier, $dto);
        return $this->json($result, 201);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('/show-for-patient', methods: ['GET'])]
    public function showForPatient(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1); // ?page=1
        $patientIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->medicalRecordService->showForPatient($patientIdentifier, $page);
        return $this->json($result, 200);
    }

    // todo show for doctor

    // todo show one (for patient, for doctor)

    // todo  update medicalRecord (doctor)
}