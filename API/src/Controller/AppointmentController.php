<?php

namespace App\Controller;

use App\Dto\Appointment\RequestAppointmentDto;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\AppointmentService;
use App\Utils\DtoInspector;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/appointment')]
class AppointmentController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly AppointmentService $appointmentService
    ) {}

    /**
     * @throws ValidationException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('/find-time', methods: ['POST'])]
    public function showFreeHours(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), RequestAppointmentDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->appointmentService->showFreeHours($dto);
        return $this->json($result, 200);
    }

    /**
     * @throws ValidationException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('/create', methods: ['POST'])]
    public function create(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), RequestAppointmentDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->appointmentService->create($dto, $userIdentifier);
        return $this->json($result, 201);
    }
}