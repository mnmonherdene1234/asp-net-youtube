using System.ComponentModel.DataAnnotations;

namespace AspNetYoutube.Models;

public class Video : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Url { get; set; }
    public string? UserId { get; set; }
    public User? User { get; set; }
}