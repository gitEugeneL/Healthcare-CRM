<?php

namespace App\Controller;

use App\Dto\Specialization\CreateSpecializationDto;
use App\Dto\Specialization\IncludeExcludeSpecializationDto;
use App\Dto\Specialization\ResponseSpecializationDto;
use App\Dto\Specialization\UpdateSpecializationDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\SpecializationService;
use App\Utils\DtoInspector;
use Nelmio\ApiDocBundle\Annotation\Model;
use Nelmio\ApiDocBundle\Annotation\Security;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\ExpressionLanguage\Expression;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;
use OpenApi\Attributes as OA;

#[OA\Tag(name: 'specializations')]
#[Route('/api/specializations')]
class SpecializationController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly SpecializationService $specializationService
    ) {}

    /**
     * @throws AlreadyExistException
     * @throws ValidationException
     */
    #[Security(name: 'MANAGER')]
    #[OA\RequestBody(
        content: new Model(type: CreateSpecializationDto::class)
    )]
    #[OA\Response(
        response: 201,
        description: 'Specialization has been successfully created',
        content: new Model(type: ResponseSpecializationDto::class)
    )]
    #[IsGranted(Roles::MANAGER)]
    #[Route('', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateSpecializationDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->specializationService->create($dto);
        return $this->json($result, 201);
    }

    #[Security(name: 'MANAGER')]
    #[Security(name: 'PATIENT')]
    #[OA\Response(
        response: 200,
        description: 'Successful response',
        content: new Model(type: ResponseSpecializationDto::class)
    )]
    #[IsGranted(new Expression('is_granted("'.Roles::PATIENT.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('', methods: ['GET'])]
    public function show(): JsonResponse
    {
        $result = $this->specializationService->show();
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[Security(name: 'MANAGER')]
    #[OA\RequestBody(
        content: new Model(type: UpdateSpecializationDto::class)
    )]
    #[OA\Response(
        response: 200,
        description: 'specialization has been successfully updated',
        content: new Model(type: ResponseSpecializationDto::class)
    )]
    #[IsGranted(Roles::MANAGER)]
    #[Route('/{specializationName}', methods: ['PUT'])]
    public function update(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), UpdateSpecializationDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->specializationService->update($dto, $request->get('specializationName'));
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     */
    #[Security(name: 'MANAGER')]
    #[OA\Response(
        response: 204,
        description: 'specialization has been successfully deleted',
    )]
    #[IsGranted(Roles::MANAGER)]
    #[Route('/{specializationName}', methods: ['DELETE'])]
    public function delete(Request $request): JsonResponse
    {
        $this->specializationService->delete($request->get('specializationName'));
        return $this->json(null, 204);
    }

    /**
     * @throws NotFoundException
     * @throws AlreadyExistException
     * @throws ValidationException
     */
    #[Security(name: 'MANAGER')]
    #[OA\RequestBody(
        content: new Model(type: IncludeExcludeSpecializationDto::class)
    )]
    #[OA\Response(
        response: 201,
        description: 'Doctor successfully included',
    )]
    #[IsGranted(Roles::MANAGER)]
    #[Route('/include-doctor', methods: ['PATCH'])]
    public function includeDoctor(Request $request): JsonResponse
    {
        $dto = $this->serializer
            ->deserialize($request->getContent(), IncludeExcludeSpecializationDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $this->specializationService->includeDoctor($dto);
        return $this->json('Doctor successfully included', 201);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[Security(name: 'MANAGER')]
    #[OA\RequestBody(
        content: new Model(type: IncludeExcludeSpecializationDto::class)
    )]
    #[OA\Response(
        response: 201,
        description: 'Doctor successfully excluded',
    )]
    #[IsGranted(Roles::MANAGER)]
    #[Route('/exclude-doctor', methods: ['PATCH'])]
    public function excludeDoctor(Request $request): JsonResponse
    {
        $dto = $this->serializer
            ->deserialize($request->getContent(), IncludeExcludeSpecializationDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $this->specializationService->excludeDoctor($dto);
        return $this->json('Doctor successfully excluded', 201);
    }
}