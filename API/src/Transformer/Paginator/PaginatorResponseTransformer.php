<?php

namespace App\Transformer\Paginator;

class PaginatorResponseTransformer
{
    public function transformToArray(iterable $items, int $currentPage, int $totalPage): array
    {
        return [
            'currentPage' => $currentPage,
            'totalPages' => $totalPage,
            'items' => $items

        ];
    }
}