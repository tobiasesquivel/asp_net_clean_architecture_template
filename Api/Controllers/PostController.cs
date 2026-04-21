using System.Security.Claims;
using System.Text.Json;
using api.Dtos.Requests;
using api.ExtensionMethods;
using api.Interfaces;
using api.Mediator.Commands;
using api.Mediator.Queries;
using api.Mediator.Results;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IMessageBus _messageBus;
    private readonly IStorageService _storageService;

    public PostController(IMessageBus messageBus, IStorageService storageService)
    {
        _messageBus = messageBus;
        _storageService = storageService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        GetPostQuery query = new GetPostQuery(id);
        ErrorOr<GetPostQueryResult> result = await _messageBus.InvokeAsync<ErrorOr<GetPostQueryResult>>(query);
        IActionResult response = result.Match<IActionResult>(
            Ok,
            BadRequest
        );
        return response;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Upload([FromForm] string description, [FromForm] IFormFile[] files)
    {
        var uploadResult = await _storageService.UploadManyAsync(files);
        if (uploadResult.IsError)
            return Problem("There were problems with the files upload");

        var getUserIdResult = User.GetUserId();
        if (getUserIdResult.IsError)
            return Unauthorized(getUserIdResult.Errors);

        var command = new UploadPostCommand(
            getUserIdResult.Value,
            uploadResult.Value,
            description
        );

        var result = await _messageBus.InvokeAsync<ErrorOr<int>>(command);

        return result.Match(
            id => CreatedAtAction(nameof(GetById), new { id = id }, new { Id = id }),
            errors => Problem(errors.ToString())
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        DeletePostCommand command = new(id);
        ErrorOr<Deleted> result = await _messageBus.InvokeAsync<ErrorOr<Deleted>>(command);

        IActionResult response = result.Match<IActionResult>(
            value => Ok(value),
            error => BadRequest(error)
        );

        return response;
    }
}