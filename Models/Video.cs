
namespace AspNetYoutube.Models;

public class Video : BaseEntity
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}