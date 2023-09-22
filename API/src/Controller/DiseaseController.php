<?php

namespace App\Controller;

use App\Dto\Disease\CreateDiseaseDto;
use App\Exception\AlreadyExistException;
use App\Exception\DtoRequestException;
use App\Exception\NotFoundException;
use App\Service\DiseaseService;
use App\Validator\DtoValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
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
     * @throws DtoRequestException
     * @throws AlreadyExistException
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
        $id = $request->get('diseaseId');
        if (!is_numeric($id))
            $this->json('Incorrect ID format. ID should be a number.', 422);
        $this->diseaseService->delete($id);
        return $this->json('successfully deleted', 204);
    }
}