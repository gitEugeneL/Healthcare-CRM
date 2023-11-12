<?php

namespace App\Controller;

use App\Dto\Address\RequestAddressDto;
use App\Dto\Patient\ResponsePatientDto;
use App\Entity\User\Roles;
use App\Exception\NotFoundException;
use App\Exception\ValidationException;
use App\Service\AddressService;
use App\Utils\DtoInspector;
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

#[OA\Tag(name: 'addresses')]
#[Route('/api/addresses')]
class AddressController extends AbstractController
{
    public function __construct(
        private readonly SerializerInterface $serializer,
        private readonly DtoInspector $dtoInspector,
        private readonly AddressService $addressService
    ) {}

    /**
     * @throws ValidationException
     * @throws NonUniqueResultException
     * @throws NotFoundException
     */
    #[Security(name: 'PATIENT')]
    #[OA\RequestBody(
        content: new Model(type: RequestAddressDto::class)
    )]
    #[OA\Response(
        response: 200,
        description: 'Successful update address',
        content: new Model(type: ResponsePatientDto::class)
    )]
    #[IsGranted(Roles::PATIENT)]
    #[Route('', methods: ['PUT'])]

    public function update(Request $request, TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $dto = $this->serializer->deserialize($request->getContent(), RequestAddressDto::class, 'json');
        $this->dtoInspector->inspect($dto);
        $result = $this->addressService->update($userIdentifier, $dto);
        return $this->json($result, 200);
    }
}
