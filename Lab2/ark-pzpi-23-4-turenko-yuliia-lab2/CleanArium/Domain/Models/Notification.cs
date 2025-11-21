namespace Domain.Models;

public class Notification : ModelBase
{
    public long UserId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; }
}
