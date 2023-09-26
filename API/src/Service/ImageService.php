<?php

namespace App\Service;

use App\Entity\Image;
use App\Entity\User\User;
use App\Exception\NotFoundException;
use App\Exception\UnsupportedMediaType;
use App\Repository\ImageRepository;
use App\Repository\UserRepository;

class ImageService
{
    private const MAX_FILE_SIZE = 1048576; // 1mb
    private const ALLOWED_FILE_FORMATS = ['jpg', 'jpeg', 'png'];

    public function __construct(
        private readonly UserRepository $userRepository,
        private readonly ImageRepository $imageRepository
    ) {}

    private function compressImage($imageFile): string
    {
        $array = array();
        foreach (str_split(file_get_contents($imageFile)) as $byte) {
            $array[] = ord($byte);
        }
        return implode(array_map('chr', $array));
    }

    private function decompressImage(Image $image): string
    {
        /** @var resource $stream */
        $stream = $image->getImageData();
        $imageData = stream_get_contents($stream);
        fclose($stream);
        return $imageData;
    }

    /**
     * @throws NotFoundException
     */
    private function findUser(string $userIdentifier): User
    {
        $user = $this->userRepository->findOneByEmail($userIdentifier);
        if (is_null($user))
            throw new NotFoundException("This User doesn't exist");
        return $user;
    }

    /**
     * @throws NotFoundException
     */
    private function getImageResource(User $user): Image
    {
        $imageResource = $user->getImage();
        if (is_null($imageResource))
            throw new NotFoundException('Image file does not exist');
        return $imageResource;
    }

    /**
     * @throws NotFoundException
     * @throws UnsupportedMediaType
     */
    public function upload(mixed $imageFile, string $userIdentifier): string
    {
        $user = $this->findUser($userIdentifier);

        if (is_null($imageFile))
            throw new NotFoundException('Image file does not exist');

        $fileFormat = $imageFile->guessExtension();
        if (!in_array($fileFormat, self::ALLOWED_FILE_FORMATS))
            throw new UnsupportedMediaType("{$fileFormat} format is not supported");

        $fileSize = $imageFile->getSize();
        if ($fileSize > self::MAX_FILE_SIZE)
            throw new UnsupportedMediaType("The image should be no more than 1MB");

        $oldImage = $user->getImage();
        if (!is_null($oldImage)) {
            $user->setImage(null);
            $this->imageRepository->remove($oldImage, true);
        }

        $image = (new Image())
            ->setName("photo-{$userIdentifier}")
            ->setType($fileFormat)
            ->setImageData($this->compressImage($imageFile))
            ->setUser($user);
        $this->imageRepository->save($image, true);
        return $image->getImageData();
    }

    /**
     * @throws NotFoundException
     */
    public function download(string $userIdentifier): string
    {
        $user = $this->findUser($userIdentifier);
        $imageResource = $this->getImageResource($user);
        return $this->decompressImage($imageResource);
    }

    /**
     * @throws NotFoundException
     */
    public function delete(string $userIdentifier): void
    {
       $user = $this->findUser($userIdentifier);
       $imageResource = $this->getImageResource($user);
       $this->imageRepository->remove($imageResource, true);
    }
}