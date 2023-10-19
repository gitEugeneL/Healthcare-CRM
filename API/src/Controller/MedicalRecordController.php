<?php

namespace App\Controller;

use App\Dto\MedicalRecord\RequestMedicalRecordDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\MedicalRecordService;
use App\Utils\DtoInspector;
use Doctrine\ORM\NonUniqueResultException;
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
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    private function showForPatientOrDoctor(
        TokenStorageInterface $tokenStorage, Request $request, string $userType): JsonResponse
    {
        $page = $request->query->getInt('page', 1); // ?page=1
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->medicalRecordService->showForPatientOrDoctor($userIdentifier, $page, $userType);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     * @throws AlreadyExistException
     * @throws NonUniqueResultException
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
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('/show-for-patient', methods: ['GET'])]
    public function showForPatient(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->showForPatientOrDoctor($tokenStorage, $request, 'patient');
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/show-for-doctor', methods: ['GET'])]
    public function showForDoctor(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->showForPatientOrDoctor($tokenStorage, $request, 'doctor');
    }


    // todo show one (for patient, for doctor) --------------------------------------------------------------------

    // todo  update medicalRecord (doctor) ------------------------------------------------------------------------
}