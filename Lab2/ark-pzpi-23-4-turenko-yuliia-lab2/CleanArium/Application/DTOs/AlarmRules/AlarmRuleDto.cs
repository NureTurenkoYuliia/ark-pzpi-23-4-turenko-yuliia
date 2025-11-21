using Domain.Enums;

namespace Application.DTOs.AlarmRules;

public class AlarmRuleDto
{
    public long Id { get; set; }
    public long DeviceId { get; set; }
    public ParameterType Parameter { get; set; }
    public ConditionType Condition { get; set; }
    public float Threshold { get; set; }
    public string Unit { get; set; }
}
