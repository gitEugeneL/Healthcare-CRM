<?php

namespace App\Controller;

use App\Dto\Doctor\CreateDoctorDto;
use App\Dto\Doctor\UpdateDoctorDto;
use App\Dto\Doctor\UpdateStatusDoctorDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\DoctorService;
use App\Utils\DtoInspector;
use App\Utils\QueryParamsInspector;
use Doctrine\ORM\NonUniqueResultException;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\ExpressionLanguage\Expression;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/doctors')]
class DoctorController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly DoctorService $doctorService,
        private readonly QueryParamsInspector $paramsInspector
    ) {}

    /**
     * @throws ValidationException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateDoctorDto::class, 'json');
        $this->dtoInspector->inspect($dto);

        $result = $this->doctorService->create($dto);
        return $this->json($result, 201);
    }

    #[IsGranted(Roles::MANAGER)]
    #[Route('/', methods: ['GET'])]
    public function show(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1); // /api/doctor/show?page=1
        $result = $this->doctorService->show($page);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('/{doctorId}', methods: ['GET'])]
    public function showOne(Request $request): JsonResponse
    {
        $doctorId = (int) $request->get('doctorId');
        $this->paramsInspector->inspect($doctorId);
        $result = $this->doctorService->showOne($doctorId);
        return $this->json($result, 200);
    }

    #[IsGranted(new Expression('is_granted("'.Roles::PATIENT.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('/show-by-specialization/{specializationName}')]
    public function showBySpecialization(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1);
        $specializationName = $request->get('specializationName');
        $result = $this->doctorService->showBySpecialization(ucfirst(strtolower(trim($specializationName))), $page);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(new Expression('is_granted("'.Roles::PATIENT.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('/show-by-disease/{diseaseId}')]
    public function showByDisease(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1);
        $diseaseId = (int) $request->get('diseaseId');
        $this->paramsInspector->inspect($diseaseId);
        $result = $this->doctorService->showByDisease($diseaseId, $page);
        return $this->json($result, 200);
    }

    /**
     * @throws AlreadyExistException
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('/update-status', methods: ['PATCH'])]
    public function updateStatus(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateStatusDoctorDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $this->doctorService->updateStatus($dto);
        return $this->json('Successfully updated', 200);
    }

    /**
     * @throws ValidationException
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('', methods: ['PATCH'])]
    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateDoctorDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->doctorService->update($dto, $userIdentifier);
        return $this->json($result, 200);
    }
}