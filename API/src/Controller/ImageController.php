<?php

namespace App\Controller;

use App\Exception\NotFoundException;
use App\Exception\UnsupportedMediaType;
use App\Service\ImageService;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Annotation\Route;
use Symfony\Component\Security\Core\Authentication\Token\Storage\TokenStorageInterface;
use Symfony\Component\Security\Http\Attribute\IsGranted;

#[Route('/api/image')]
class ImageController extends AbstractController
{
    public function __construct(
        private readonly ImageService $imageService
    ) {}

    private function makeResponse($image): Response
    {
        $response = new Response();
        $response->headers->set('Content-Type', 'image/png');
        $response->setContent($image);
        return $response;
    }

    /**
     * @throws NotFoundException
     * @throws UnsupportedMediaType
     */
    #[IsGranted('IS_AUTHENTICATED_FULLY')]
    #[Route('/upload', methods: ['POST'])]
    public function uploadImage(TokenStorageInterface $tokenStorage, Request $request): Response
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $imageFile = $request->files->get('image');
        $result = $this->imageService->upload($imageFile, $userIdentifier);
        return $this->makeResponse($result);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted('IS_AUTHENTICATED_FULLY')]
    #[Route('/download', methods: ['GET'])]
    public function downloadImage(TokenStorageInterface $tokenStorage): Response
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $result = $this->imageService->download($userIdentifier);
        return $this->makeResponse($result);
    }

    /**
     * @throws NotFoundException
     */
    #[IsGranted('IS_AUTHENTICATED_FULLY')]
    #[Route('/delete', methods: ['DELETE'])]
    public function deleteImage(TokenStorageInterface $tokenStorage): JsonResponse
    {
        $userIdentifier = $tokenStorage->getToken()->getUser()->getUserIdentifier();
        $this->imageService->delete($userIdentifier);
        return $this->json('successfully deleted', 204);
    }
}