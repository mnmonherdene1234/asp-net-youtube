using System.ComponentModel.DataAnnotations;

namespace AspNetYoutube.Models;

public class Comment : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public string? Text { get; set; }
    public Guid VideoId { get; set; }
    public string? UserId { get; set; }

    public Video? Video { get; set; }
    public User? User { get; set; }
}