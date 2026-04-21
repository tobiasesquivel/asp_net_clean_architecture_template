namespace api.Controllers;

using api.Dtos.Requests;
using api.Mediator.Commands;
using api.Models.Common;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMessageBus _messageBus;

    public AuthController(IMessageBus messageBus)
    {
        this._messageBus = messageBus;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("ok");
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        RegisterCommand command = new(
            request.Username,
            request.Email,
            request.Displayname,
            request.Password
        );

        ErrorOr<AuthenticatedUser> authenticatedUserResult = await _messageBus.InvokeAsync<ErrorOr<AuthenticatedUser>>(command);
        if (authenticatedUserResult.IsError) return BadRequest(authenticatedUserResult.Errors.ToString());
        return Ok(authenticatedUserResult.Value);
    }


}