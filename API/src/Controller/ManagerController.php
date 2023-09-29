<?php

namespace App\Controller;

use App\Dto\Manager\CreateManagerDto;
use App\Dto\Manager\UpdateManagerDto;
use App\Exception\AlreadyExistException;
use App\Exception\DtoRequestException;
use App\Exception\NotFoundException;
use App\Service\ManagerService;
use App\Validator\DtoValidator;
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
        private readonly DtoValidator $dtoValidator,
        private readonly ManagerService $managerService
    ) {}

    /**
     * @throws DtoRequestException
     * @throws AlreadyExistException
     */
    #[IsGranted('ROLE_ADMIN')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateManagerDto::class, 'json');
        $this->dtoValidator->validate($dto);

        $result = $this->managerService->create($dto);
        return $this->json($result, 201);
    }

    /**
     * @throws DtoRequestException
     * @throws NotFoundException
     */
    #[Route('/update', methods: ['PATCH'])]
    #[IsGranted('ROLE_MANAGER')]
    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateManagerDto::class, 'json');
        $this->dtoValidator->validate($dto);
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->managerService->update($dto, $userIdentifier);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/info', methods: ['GET'])]
    public function info(TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->managerService->info($userIdentifier);
        return $this->json($result, 200);
    }
}