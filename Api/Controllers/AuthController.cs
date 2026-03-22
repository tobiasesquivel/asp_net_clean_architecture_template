namespace Api.Controllers;

using Api.Dtos.Requests;
using Application.Auth.Commands.Register;
using Application.Auth.Common;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        RegisterCommand command = new(
            request.Username,
            request.Email,
            request.DisplayName,
            request.Password
        );

        ErrorOr<AuthenticationResult> result = await _mediator.SendAsync(command);

        return Ok(result);
    }
}