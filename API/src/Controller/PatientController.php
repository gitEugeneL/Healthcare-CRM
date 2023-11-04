<?php

namespace App\Controller;

use App\Dto\Patient\CreatePatientDto;
use App\Dto\Patient\UpdatePatientDto;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\PatientService;
use App\Utils\DtoInspector;
use App\Utils\QueryParamsInspector;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\ExpressionLanguage\Expression;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/patients')]
class PatientController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly PatientService $patientService,
        private readonly QueryParamsInspector $paramsInspector
    ) {}

    /**
     * @throws ValidationException
     */
    #[Route('', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreatePatientDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->patientService->create($dto);
        return $this->json($result, 201);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('', methods: ['PATCH'])]
    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), UpdatePatientDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->patientService->update($dto, $userIdentifier);
        return $this->json($result, 200);
    }

    #[IsGranted(new Expression('is_granted("'.Roles::DOCTOR.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('/', methods: ['GET'])]
    public function show(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1);
        $result = $this->patientService->show($page);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(new Expression('is_granted("'.Roles::DOCTOR.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('/{patientId}', methods: ['GET'])]
    public function showOne(Request $request): JsonResponse
    {
        $patientId = (int) $request->get('patientId');
        $this->paramsInspector->inspect($patientId);
        $result = $this->patientService->showOne($patientId);
        return $this->json($result, 200);
    }
}