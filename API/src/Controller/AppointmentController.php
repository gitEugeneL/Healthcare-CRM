<?php

namespace App\Controller;

use App\Dto\Appointment\RequestAppointmentDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\AccessException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\AppointmentService;
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

#[Route('/api/appointment')]
class AppointmentController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly AppointmentService $appointmentService,
        private readonly QueryParamsInspector $paramsInspector
    ) {}

    /**
     * @throws ValidationException
     */
    private function showForUser(TokenStorageInterface $tokenStorage, Request $request, string $action): JsonResponse
    {
        $dateString = $request->query->getString('date'); // ?date=2030-12-31
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->appointmentService->showForUser($userIdentifier, $dateString, $action);
        return $this->json($result, 200);
    }

    /**
     * @throws AccessException
     * @throws AlreadyExistException
     * @throws NotFoundException
     * @throws ValidationException
     */
    private function update(TokenStorageInterface $tokenStorage, Request $request, string $action): JsonResponse
    {
        $appointmentId = (int) $request->get('appointmentId');
        $this->paramsInspector->inspect($appointmentId);
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->appointmentService->update($appointmentId, $userIdentifier, $action);
        return $this->json($result, 200);
    }

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

    /**
     * @throws ValidationException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('/show-for-manager', methods: ['GET'])]
    public function showForManager(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->showForUser($tokenStorage, $request, 'manager');
    }

    /**
     * @throws ValidationException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/show-for-doctor', methods: ['GET'])]
    public function showForDoctor(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->showForUser($tokenStorage, $request, 'doctor');
    }

    /**
     * @throws ValidationException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('/show-for-patient', methods: ['GET'])]
    public function showForPatient(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->showForUser($tokenStorage, $request, 'patient');
    }

    /**
     * @throws AccessException
     * @throws NotFoundException
     * @throws ValidationException
     * @throws AlreadyExistException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/{appointmentId}/finalize', methods: ['PATCH'])]
    public function finalize(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->update($tokenStorage, $request, 'finalize');
    }

    /**
     * @throws AccessException
     * @throws NotFoundException
     * @throws ValidationException
     * @throws AlreadyExistException
     */
    #[IsGranted(new Expression('is_granted("'.Roles::DOCTOR.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('/{appointmentId}/cancel', methods: ['PATCH'])]
    public function cancel(TokenStorageInterface $tokenStorage, Request $request): JsonResponse
    {
        return $this->update($tokenStorage, $request, 'cancel');
    }
}