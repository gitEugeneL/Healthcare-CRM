<?php

namespace App\Controller;

use App\Dto\Specialization\CreateSpecializationDto;
use App\Exception\AlreadyExistException;
use App\Exception\DtoRequestException;
use App\Exception\NotFoundException;
use App\Service\SpecializationService;
use App\Validator\DtoValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;
use Symfony\Component\ExpressionLanguage\Expression;

#[Route('/api/specialization')]
class SpecializationController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoValidator $dtoValidator,
        private readonly SpecializationService $specializationService
    ) {}

    /**
     * @throws DtoRequestException
     * @throws AlreadyExistException
     */
    #[IsGranted('ROLE_MANAGER')]
    #[Route('/create', methods: ['POST'])]
    public function create(Request $request): JsonResponse
    {
        $dto = $this->serializer->deserialize($request->getContent(), CreateSpecializationDto::class, 'json');
        $this->dtoValidator->validate($dto);

        $result = $this->specializationService->create($dto);
        return $this->json($result, 201);
    }

    #[IsGranted(new Expression('is_granted("ROLE_PATIENT") or is_granted("ROLE_MANAGER")'))]
    #[Route('/show', methods: ['GET'])]
    public function show(): JsonResponse
    {
        $result = $this->specializationService->show();
        return $this->json($result, 200);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted(new Expression('is_granted("ROLE_PATIENT") or is_granted("ROLE_MANAGER")'))]
    #[Route('/show/doctors/{specializationName}', methods: ['GET'])]
    public function showDoctors(Request $request): JsonResponse
    {
        $specializationName = $request->get('specializationName');
        $result = $this->specializationService->showDoctors($specializationName);
        return $this->json($result, 200);
    }

    // todo include doctor (specialization)
    // todo exclude doctor (specialization)
}