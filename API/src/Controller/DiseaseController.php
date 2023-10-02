<?php

namespace App\Controller;

use App\Dto\Disease\CreateDiseaseDto;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\DiseaseService;
use App\Validator\DtoValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/disease')]
class DiseaseController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoValidator $dtoValidator,
        private readonly DiseaseService $diseaseService
    ) {}

    /**
     * @throws AlreadyExistException
     * @throws ValidationException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateDiseaseDto::class, 'json');
        $this->dtoValidator->validate($dto);
        $result = $this->diseaseService->create($dto);
        return $this->json($result, 201);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/delete/{diseaseId}', methods: ['DELETE'])]
    public function delete(Request $request): JsonResponse
    {
        $diseaseId = (int) $request->get('diseaseId');
        $this->diseaseService->delete($diseaseId);
        return $this->json('successfully deleted', 204);
    }

    /**
     * @throws AlreadyExistException
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_DOCTOR')]
    #[Route('/add-doctor/{diseaseId}', methods: ['PATCH'])]
    public function addDoctor(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $diseaseId = (int) $request->get('diseaseId');
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $this->diseaseService->addDoctor($doctorIdentifier, $diseaseId);
        return $this->json('Doctor successfully added', 201);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted('ROLE_DOCTOR')]
    #[Route('/remove-doctor/{diseaseId}', methods: ['PATCH'])]
    public function removeDoctor(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $diseaseId = (int) $request->get('diseaseId');
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $this->diseaseService->removeDoctor($doctorIdentifier, $diseaseId);
        return $this->json('Doctor successfully removed', 201);
    }
}