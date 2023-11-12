<?php

namespace App\Controller;

use App\Dto\MedicalRecord\CreateMedicalRecordDto;
use App\Dto\MedicalRecord\ResponseMedicalRecordDto;
use App\Dto\MedicalRecord\UpdateMedicalRecordDto;
use App\Entity\User\Roles;
use App\Exception\AccessException;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\MedicalRecordService;
use App\Utils\DtoInspector;
use App\Utils\QueryParamsInspector;
use Doctrine\ORM\NonUniqueResultException;
use Nelmio\ApiDocBundle\Annotation\Model;
use Nelmio\ApiDocBundle\Annotation\Security;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;
use OpenApi\Attributes as OA;

#[OA\Tag(name: 'medicalRecord')]
#[Route('/api/medical-records')]
class MedicalRecordController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly QueryParamsInspector $paramsInspector,
        private readonly MedicalRecordService $medicalRecordService,
    ) {}

    /**
     * @throws NotFoundException
     * @throws ValidationException
     * @throws AlreadyExistException
     * @throws NonUniqueResultException
     */
    #[Security(name: 'DOCTOR')]
    #[OA\RequestBody(
        content: new Model(type: CreateMedicalRecordDto::class)
    )]
    #[OA\Response(
        response: 201,
        description: 'Medical record has been successfully created',
        content: new Model(type: ResponseMedicalRecordDto::class)
    )]
    #[IsGranted(Roles::DOCTOR)]
    #[Route('', methods: ['POST'])]
    public function create(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), CreateMedicalRecordDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->medicalRecordService->create($doctorIdentifier, $dto);
        return $this->json($result, 201);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[Security(name: 'DOCTOR')]
    #[OA\QueryParameter(name: 'page')]
    #[OA\Response(
        response: 200,
        description: 'Successful response with pagination',
        content: new Model(type: ResponseMedicalRecordDto::class)
    )]
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/for-doctor/{patientId}', methods: ['GET'])]
    public function showForDoctor(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1); // ?page=1
        $patientId = (int) $request->get('patientId');
        $this->paramsInspector->inspect($patientId);
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->medicalRecordService->showForDoctor($doctorIdentifier, $patientId, $page);
        return $this->json($result, 200);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[Security(name: 'PATIENT')]
    #[OA\QueryParameter(name: 'page')]
    #[OA\Response(
        response: 200,
        description: 'Successful response with pagination',
        content: new Model(type: ResponseMedicalRecordDto::class)
    )]
    #[IsGranted(Roles::PATIENT)]
    #[Route('/for-patient', methods: ['GET'])]
    public function showForPatient(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1); // ?page=1
        $patientIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->medicalRecordService->showForPatient($patientIdentifier, $page);
        return $this->json($result, 200);
    }

    /**
     * @throws ValidationException
     * @throws NotFoundException
     */
    #[Security(name: 'DOCTOR')]
    #[OA\Response(
        response: 200,
        description: 'Successful response',
        content: new Model(type: ResponseMedicalRecordDto::class)
    )]
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/{medicalRecordId}/for-doctor', methods: ['GET'])]
    public function showOneForDoctor(Request $request): JsonResponse
    {
        $medicalRecordId = (int) $request->get('medicalRecordId');
        $this->paramsInspector->inspect($medicalRecordId);
        $result = $this->medicalRecordService->showOneForDoctor($medicalRecordId);
        return $this->json($result, 200);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[Security(name: 'PATIENT')]
    #[OA\Response(
        response: 200,
        description: 'Successful response',
        content: new Model(type: ResponseMedicalRecordDto::class)
    )]
    #[IsGranted(Roles::PATIENT)]
    #[Route('/{medicalRecordId}/for-patient', methods: ['GET'])]
    public function showOneForPatient(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $medicalRecordId = (int) $request->get('medicalRecordId');
        $this->paramsInspector->inspect($medicalRecordId);
        $patientIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->medicalRecordService->showOneForPatient($patientIdentifier, $medicalRecordId);
        return $this->json($result, 200);
    }

    /**
     * @throws AccessException
     * @throws NotFoundException
     * @throws ValidationException
     * @throws NonUniqueResultException
     */
    #[Security(name: 'DOCTOR')]
    #[OA\RequestBody(
        content: new Model(type: UpdateMedicalRecordDto::class)
    )]
    #[OA\Response(
        response: 200,
        description: 'Medical record status has been successfully updated',
        content: new Model(type: ResponseMedicalRecordDto::class)
    )]
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/{medicalRecordId}', methods: ['PATCH'])]
    public function update(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $medicalRecordId = (int) $request->get('medicalRecordId');
        $this->paramsInspector->inspect($medicalRecordId);
        $dto = $this->serializer->deserialize($request->getContent(), UpdateMedicalRecordDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->medicalRecordService->update($doctorIdentifier, $medicalRecordId, $dto);
        return $this->json($result, 200);
    }
}