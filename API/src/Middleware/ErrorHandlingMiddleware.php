<?php

namespace App\Middleware;

use App\Exception\AlreadyExistException;
use App\Exception\DtoRequestException;
use App\Exception\NotFoundException;
use Symfony\Component\EventDispatcher\EventSubscriberInterface;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\HttpKernel\Event\ExceptionEvent;
use Symfony\Component\HttpKernel\KernelEvents;

class ErrorHandlingMiddleware implements EventSubscriberInterface
{
    public static function getSubscribedEvents(): array
    {
        return [KernelEvents::EXCEPTION => 'onKernelException'];
    }

    public function onKernelException(ExceptionEvent $event): void
    {
        $exception = $event->getThrowable();

        if ($exception instanceof AlreadyExistException) {
            $response = new Response($exception->getMessage(),409);
        }
        elseif ($exception instanceof NotFoundException) {
            $response = new Response($exception->getMessage(), 404);
        }
        elseif ($exception instanceof DtoRequestException) {
            $response = new Response($exception->getMessage(), 422);
        }
        else {
            $response = new Response($exception, 500); // only for dev
            // $response = new Response('Something went wrong', 500);
        }
        $event->setResponse($response);
    }
}