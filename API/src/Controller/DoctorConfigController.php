<?php

namespace App\Controller;

use App\Dto\DoctorConfig\RequestDoctorConfigDto;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\DoctorConfigService;
use App\Utils\DtoInspector;
use Doctrine\ORM\NonUniqueResultException;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/doctor-config')]
class DoctorConfigController extends AbstractController
{
    public function __construct(
        private readonly DoctorConfigService $doctorConfigService,
        private readonly DtoInspector $dtoInspector,
        private readonly SerializerInterface $serializer,
    ) {}

    /**
     * @throws ValidationException
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::DOCTOR)]
    #[Route('', methods: ['PUT'])]
    public function config(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $doctorIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), RequestDoctorConfigDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->doctorConfigService->config($doctorIdentifier, $dto);
        return $this->json($result, 200);
    }
}