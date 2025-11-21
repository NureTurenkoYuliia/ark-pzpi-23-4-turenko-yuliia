using Application.DTOs.AlarmRules;
using MediatR;

namespace Application.AlarmRules.Queries.GetById;

public record GetAlarmRuleByIdQuery(long UserId, long RuleId) : IRequest<AlarmRuleDto>;
