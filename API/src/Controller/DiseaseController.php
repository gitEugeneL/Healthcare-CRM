<?php

namespace App\Controller;

use App\Dto\Disease\RequestDiseaseDto;
use App\Dto\Disease\ResponseDiseaseDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\DiseaseService;
use App\Utils\DtoInspector;
use App\Utils\QueryParamsInspector;
use Doctrine\ORM\NonUniqueResultException;
use Nelmio\ApiDocBundle\Annotation\Model;
use Nelmio\ApiDocBundle\Annotation\Security;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;
use OpenApi\Attributes as OA;

#[OA\Tag(name: 'diseases')]
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
    #[Security(name: 'MANAGER')]
    #[OA\RequestBody(
        content: new Model(type: RequestDiseaseDto::class)
    )]
    #[OA\Response(
        response: 201,
        description: 'Disease has been successfully created',
        content: new Model(type: ResponseDiseaseDto::class)
    )]
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

    #[Security(name: 'MANAGER')]
    #[OA\Response(
        response: 204,
        description: 'Disease has been successfully deleted',
    )]
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

    #[Security(name: 'DOCTOR')]
    #[OA\Response(
        response: 201,
        description: 'Doctor successfully added',
    )]
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
    #[Security(name: 'DOCTOR')]
    #[OA\Response(
        response: 201,
        description: 'Doctor successfully removed',
    )]
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