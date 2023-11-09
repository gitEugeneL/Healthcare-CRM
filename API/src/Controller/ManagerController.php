<?php

namespace App\Controller;

use App\Dto\Manager\CreateManagerDto;
use App\Dto\Manager\UpdateManagerDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\ManagerService;
use App\Utils\DtoInspector;
use Doctrine\ORM\NonUniqueResultException;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/managers')]
class ManagerController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly ManagerService $managerService
    ) {}

    /**
     * @throws AlreadyExistException
     * @throws ValidationException
     */
    #[IsGranted(Roles::ADMIN)]
    #[Route('', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateManagerDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->managerService->create($dto);
        return $this->json($result, 201);
    }


    /**
     * @throws ValidationException
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[Route('', methods: ['PATCH'])]
    #[IsGranted(Roles::MANAGER)]
    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateManagerDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->managerService->update($dto, $userIdentifier);
        return $this->json($result, 200);
    }
}