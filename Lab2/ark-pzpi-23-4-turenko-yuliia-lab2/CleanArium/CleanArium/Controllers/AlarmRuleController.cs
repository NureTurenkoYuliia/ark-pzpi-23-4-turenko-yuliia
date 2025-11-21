using Application.Abstractions;
using Application.AlarmRules.Commands.Delete;
using Application.AlarmRules.Commands.Update;
using Application.AlarmRules.Commands.Create;
using Application.AlarmRules.Queries.GetAllByDeviceId;
using Application.AlarmRules.Queries.GetById;
using CleanArium.Contracts.AlarmRules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArium.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlarmRuleController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public AlarmRuleController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateAlarmRuleRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new CreateAlarmRuleCommand(
            UserId: userId,
            DeviceId: request.DeviceId,
            Parameter: request.Parameter,
            Condition: request.Condition,
            Threshold: request.Threshold,
            Unit: request.Unit
        );

        var result = await _mediator.Send(command, ct);

        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateAlarmRuleRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new UpdateAlarmRuleCommand(
            UserId: userId,
            RuleId: request.RuleId,
            Parameter: request.Parameter,
            Condition: request.Condition,
            Threshold: request.Threshold,
            Unit: request.Unit
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] long ruleId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new DeleteAlarmRuleCommand(
            UserId: userId,
            RuleId: ruleId
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpGet("get-all-by-device")]
    public async Task<IActionResult> GetAllByDevice([FromBody] long deviceId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var query = new GetAlarmRulesByDeviceIdQuery(userId, deviceId);
        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }

    [HttpGet("get-by-id")]
    public async Task<IActionResult> GetById([FromBody] long ruleId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var query = new GetAlarmRuleByIdQuery(userId, ruleId);
        var result = await _mediator.Send(query, ct);

        return Ok(result);
    }
}
