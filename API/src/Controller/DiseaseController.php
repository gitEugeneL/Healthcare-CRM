<?php

namespace App\Controller;

use App\Dto\Disease\RequestDiseaseDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\DiseaseService;
use App\Utils\DtoInspector;
use App\Utils\QueryParamsInspector;
use Doctrine\ORM\NonUniqueResultException;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/diseases')]
class DiseaseController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly DiseaseService $diseaseService,
        private readonly QueryParamsInspector $paramsInspector
    ) {}

    /**
     * @throws AlreadyExistException
     * @throws ValidationException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), RequestDiseaseDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->diseaseService->create($dto);
        return $this->json($result, 201);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('/{diseaseId}', methods: ['DELETE'])]
    public function delete(Request $request): JsonResponse
    {
        $diseaseId = (int) $request->get('diseaseId');
        $this->paramsInspector->inspect($diseaseId);
        $this->diseaseService->delete($diseaseId);
        return $this->json(null, 204);
    }

    /**
     * @throws NonUniqueResultException
     * @throws AlreadyExistException
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/add-doctor/{diseaseId}', methods: ['PATCH'])]
    public function addDoctor(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $diseaseId = (int) $request->get('diseaseId');
        $this->paramsInspector->inspect($diseaseId);
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $this->diseaseService->addDoctor($doctorIdentifier, $diseaseId);
        return $this->json('Doctor successfully added', 201);
    }

    /**
     * @throws NonUniqueResultException
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('/remove-doctor/{diseaseId}', methods: ['PATCH'])]
    public function removeDoctor(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $diseaseId = (int) $request->get('diseaseId');
        $this->paramsInspector->inspect($diseaseId);
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $this->diseaseService->removeDoctor($doctorIdentifier, $diseaseId);
        return $this->json('Doctor successfully removed', 201);
    }
}