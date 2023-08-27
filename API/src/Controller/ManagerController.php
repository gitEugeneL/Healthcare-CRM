<?php

namespace App\Controller;

use App\Dto\Request\Manager\CreateManagerDto;
use App\Service\ManagerService;
use App\Validator\RequestValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/manager')]
class ManagerController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly RequestValidator $requestValidator,
        private readonly ManagerService $managerService
    ) {}

    #[IsGranted('ROLE_ADMIN')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateManagerDto::class, 'json');
        $errors = $this->requestValidator->dtoValidator($dto);
        if (count($errors) > 0)
            return $this->json($errors, 422);

        $result = $this->managerService->create($dto);
        return $this->json($result, 201);
    }

    #[IsGranted('ROLE_MANAGER')]
    #[Route('/info', methods: ['GET'])]
    public function info(TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->managerService->info($userIdentifier);
        return $this->json($result, 200);
    }
}