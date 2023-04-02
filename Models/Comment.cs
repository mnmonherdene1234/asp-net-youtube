namespace AspNetYoutube.Models;

public class Comment : BaseEntity
{
    public Guid Id { get; set; }
    public string? Text { get; set; }

    public Guid VideoId { get; set; }
    public Video? Video { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}