<?php

namespace App\Middleware;

use App\Exception\AlreadyExistException;
use App\Exception\NotFoundException;
use App\Exception\UnsupportedMediaType;
use App\Exception\ValidationException;
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
        elseif ($exception instanceof ValidationException) {
            $response = new Response($exception->getMessage(), 422);
        }
        elseif ($exception instanceof UnsupportedMediaType) {
            $response = new Response($exception->getMessage(), 415);
        }
        else {
            $response = new Response($exception, 500); // only for dev
//             $response = new Response('Something went wrong', 500);
        }
        $event->setResponse($response);
    }
}