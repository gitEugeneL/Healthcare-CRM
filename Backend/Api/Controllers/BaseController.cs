using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;

    protected string? CurrentUserId() => 
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    protected string? CurrentUserRole() =>
        User.FindFirst(ClaimTypes.Role)?.Value;
}
