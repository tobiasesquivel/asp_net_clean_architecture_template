namespace Api.Controllers.Auth;

using Application.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ISender _mediatr;

    public AuthController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        LoginResponse response = await this._mediatr.Send(request);
        return Ok(response);
    }
}