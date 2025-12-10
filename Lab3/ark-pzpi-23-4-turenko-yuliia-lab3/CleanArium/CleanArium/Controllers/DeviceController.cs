using Application.Abstractions;
using Application.Devices.Commands.Create;
using Application.Devices.Commands.Delete;
using Application.Devices.Commands.Update;
using Application.Devices.Queries.GetAllByAquariumId;
using CleanArium.Contracts.Devices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArium.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class DeviceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserService _userService;

    public DeviceController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator;
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new CreateDeviceCommand(
            UserId: userId,
            AquariumId: request.AquariumId,
            DeviceType: request.DeviceType,
            DeviceStatus: request.DeviceStatus
        );

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateDeviceRequest request, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new UpdateDeviceCommand(
            UserId: userId,
            DeviceId: request.DeviceId,
            DeviceType: request.DeviceType,
            DeviceStatus: request.DeviceStatus
        );

        await _mediator.Send(command, ct);
        return Ok();
    }

    [HttpDelete("delete/{deviceId:long}")]
    public async Task<IActionResult> Delete([FromRoute] long deviceId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var command = new DeleteDeviceCommand(
            UserId: userId,
            DeviceId: deviceId
        );

        await _mediator.Send(command, ct);

        return Ok();
    }

    [HttpGet("get-devices-by-aquarium/{aquariumId:long}")]
    public async Task<IActionResult> GetAllByAquarium(long aquariumId, CancellationToken ct)
    {
        var userId = _userService.GetApplicationUserId()!.Value;

        var query = new GetDevicesByAquariumIdQuery(
            UserId: userId,
            AquariumId: aquariumId
        );

        var result = await _mediator.Send(query, ct);
        return Ok(result);
    }
}
