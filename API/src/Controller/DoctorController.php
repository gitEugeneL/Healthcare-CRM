<?php

namespace App\Controller;

use App\Dto\Request\Doctor\CreateDoctorDto;
use App\Dto\Request\Doctor\UpdateStatusDoctorDto;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Service\DoctorService;
use App\Validator\RequestValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/doctor')]
class DoctorController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly RequestValidator $requestValidator,
        private readonly DoctorService $doctorService
    ) {}

    /**
     * @throws AlreadyExistException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateDoctorDto::class, 'json');
        $errors = $this->requestValidator->dtoValidator($dto);
        if (count($errors) > 0)
            return $this->json($errors, 422);

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

    /**
     * @throws AlreadyExistException
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/update-status', methods: ['PATCH'])]
    public function updateStatus(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateStatusDoctorDto::class, 'json');
        $errors = $this->requestValidator->dtoValidator($dto);
        if (count($errors) > 0)
            return $this->json($errors, 422);

        $this->doctorService->updateStatus($dto);
        return $this->json('Successfully updated', 204);
    }
}