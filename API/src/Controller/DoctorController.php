<?php

namespace App\Controller;

use App\Dto\Doctor\CreateDoctorDto;
use App\Dto\Doctor\UpdateDoctorDto;
use App\Dto\Doctor\UpdateStatusDoctorDto;
use App\Exception\AlreadyExistException;
use App\Exception\DtoRequestException;
use App\Exception\NotFoundException;
use App\Service\DoctorService;
use App\Validator\DtoValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\ExpressionLanguage\Expression;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;

#[Route('/api/doctor')]
class DoctorController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoValidator $dtoValidator,
        private readonly DoctorService $doctorService
    ) {}

    /**
     * @throws DtoRequestException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateDoctorDto::class, 'json');
        $this->dtoValidator->validate($dto);

        $result = $this->doctorService->create($dto);
        return $this->json($result, 201);
    }

    #[IsGranted('ROLE_MANAGER')]
    #[Route('/show', methods: ['GET'])]
    public function show(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1); // /api/doctor/show?page=1
        $result = $this->doctorService->show($page);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/show/{doctorId}', methods: ['GET'])]
    public function showOne(Request $request): JsonResponse
    {
        $result = $this->doctorService->showOne((int) $request->get('doctorId'));
        return $this->json($result, 200);
    }

    #[IsGranted(new Expression('is_granted("ROLE_PATIENT") or is_granted("ROLE_MANAGER")'))]
    #[Route('/show-by-specialization/{specializationName}')]
    public function showBySpecialization(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1);
        $specializationName = $request->get('specializationName');
        $result = $this->doctorService->showBySpecialization($specializationName, $page);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted(new Expression('is_granted("ROLE_PATIENT") or is_granted("ROLE_MANAGER")'))]
    #[Route('/show-by-disease/{diseaseId}')]
    public function showByDisease(Request $request): JsonResponse
    {
        $page = $request->query->getInt('page', 1);
        $diseaseId = (int) $request->get('diseaseId');
        $result = $this->doctorService->showByDisease($diseaseId, $page);
        return $this->json($result, 200);
    }

    /**
     * @throws AlreadyExistException
     * @throws NotFoundException
     * @throws DtoRequestException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/update-status', methods: ['PATCH'])]
    public function updateStatus(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateStatusDoctorDto::class, 'json');
        $this->dtoValidator->validate($dto);

        $this->doctorService->updateStatus($dto);
        return $this->json('Successfully updated', 200);
    }

    /**
     * @throws DtoRequestException
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_DOCTOR')]
    #[Route('/update', methods: ['PATCH'])]
    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateDoctorDto::class, 'json');
        $this->dtoValidator->validate($dto);
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->doctorService->update($dto, $userIdentifier);
        return $this->json($result, 200);
    }
}