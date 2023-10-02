<?php

namespace App\Controller;

use App\Dto\Address\UpdateAddressDto;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\AddressService;
use App\Validator\DtoValidator;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;
use Symfony\Component\Serializer\SerializerInterface;

#[Route('/api/address')]
class AddressController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoValidator $dtoValidator,
        private readonly AddressService $addressService
    ) {}

    /**
     * @throws ValidationException
     * @throws NotFoundException
     */
    #[IsGranted(Roles::PATIENT)]
    #[Route('/update', methods: ['PUT'])]
    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), UpdateAddressDto::class, 'json');
        $this->dtoValidator->validate($dto);
        $result = $this->addressService->update($userIdentifier, $dto);
        return $this->json($result, 200);
    }
}
