using Application.Abstractions;
using Application.Aquariums.Commands.Create;
using Application.Aquariums.Commands.Delete;
using Application.Aquariums.Commands.Update;
using Application.Aquariums.Queries.GetAllByUserId;
using CleanArium.Contracts.Aquariums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArium.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AquariumController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public AquariumController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateAquariumRequest request,CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new CreateAquariumCommand(
            UserId: userId,
            Name: request.Name,
            Location: request.Location
        );

        var result = await _mediator.Send(command, ct);

        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateAquariumRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new UpdateAquariumCommand(
            UserId: userId,
            AquariumId: request.AquariumId,
            Name: request.Name,
            Location: request.Location
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] long aquariumId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new DeleteAquariumCommand(
            UserId: userId,
            AquariumId: aquariumId
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpGet("get-all-by-user")]
    public async Task<IActionResult> GetAllByUser(CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var query = new GetAquariumsByUserIdQuery(userId);
        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }
}
