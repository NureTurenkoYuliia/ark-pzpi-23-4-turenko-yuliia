using Application.Abstractions;
using Application.ScheduledCommands.Commands.Create;
using Application.ScheduledCommands.Commands.Delete;
using Application.ScheduledCommands.Commands.Update;
using Application.ScheduledCommands.Queries.GetAllByDeviceId;
using Application.ScheduledCommands.Queries.GetById;
using CleanArium.Contracts.ScheduledCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArium.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduledCommandController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public ScheduledCommandController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateScheduledCommandRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new CreateScheduledCommand(
            UserId: userId,
            DeviceId: request.DeviceId,
            CommandType: request.CommandType,
            StartTime: request.StartTime,
            RepeatMode: request.RepeatMode,
            IntervalMinutes: request.IntervalMinutes,
            IsActive: request.IsActive
        );

        var result = await _mediator.Send(command, ct);

        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateScheduledCommandRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new UpdateScheduledCommand(
            UserId: userId,
            CommandId: request.Id,
            CommandType: request.CommandType,
            StartTime: request.StartTime,
            RepeatMode: request.RepeatMode,
            IntervalMinutes: request.IntervalMinutes,
            IsActive: request.IsActive
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] long scheduledCommandId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new DeleteScheduledCommand(
            UserId: userId,
            CommandId: scheduledCommandId
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpGet("get-all-by-device")]
    public async Task<IActionResult> GetAllByDevice([FromBody] long deviceId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var query = new GetScheduledCommandsByDeviceIdQuery(userId, deviceId);
        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById([FromBody] long scheduledCommandId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var query = new GetScheduledCommandByIdQuery(userId, scheduledCommandId);
        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }
}
