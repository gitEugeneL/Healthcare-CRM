<?php

namespace App\Controller;

use App\Exception\NotFoundException;
use App\Service\AccountService;
use Doctrine\ORM\NonUniqueResultException;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;

#[Route('/api/account')]
#[IsGranted('IS_AUTHENTICATED_FULLY')]
class AccountController extends AbstractController
{
    public function __construct(
        private readonly AccountService $accountService
    ) {}

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[Route('/info', methods: ['GET'])]
    public function info(TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $userRole = $tokenStorage->getToken()->getRoleNames()[0];
        $result = $this->accountService->info($userIdentifier, $userRole);
        return $this->json($result, 200);
    }

    // todo swagger
    // todo add makefile config for test db
}