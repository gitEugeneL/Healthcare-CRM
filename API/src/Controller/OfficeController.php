<?php

namespace App\Controller;

use App\Dto\Office\RequestOfficeDto;
use App\Entity\User\Roles;
use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\OfficeService;
use App\Utils\DtoInspector;
use App\Utils\QueryParamsInspector;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\ExpressionLanguage\Expression;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/offices')]
class OfficeController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly OfficeService $officeService,
        private readonly QueryParamsInspector $paramsInspector
    ) {}

    /**
     * @throws ValidationException
     * @throws AlreadyExistException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), RequestOfficeDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->officeService->create($dto);
        return $this->json($result, 201);
    }

    #[IsGranted(new Expression('is_granted("'.Roles::DOCTOR.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('', methods: ['GET'])]
    public function show(): JsonResponse
    {
        $result = $this->officeService->show();
        return $this->json($result, 200);
    }

    /**
     * @throws ValidationException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::MANAGER)]
    #[Route('', methods: ['PATCH'])]
    public function update(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), RequestOfficeDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->officeService->update($dto);
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     * @throws ValidationException
     */
    #[IsGranted(new Expression('is_granted("'.Roles::DOCTOR.'") or is_granted("'.Roles::MANAGER.'")'))]
    #[Route('/{number}', methods: ['PATCH'])]
    public function changeStatus(Request $request): JsonResponse
    {
        $number = (int) $request->get('number');
        $this->paramsInspector->inspect($number);
        $result = $this->officeService->changeStatus($number);
        return $this->json($result, 200);
    }

    /**
     * @throws ValidationException
     * @throws NotFoundException
     */
    #[Route('/{number}', methods: ['DELETE'])]
    public function delete(Request $request): JsonResponse
    {
        $number = (int) $request->get('number');
        $this->paramsInspector->inspect($number);
        $this->officeService->delete($number);
        return $this->json(null, 204);
    }

    // todo fix routes
    // todo user info
    // todo validation tests
    // todo swagger
}