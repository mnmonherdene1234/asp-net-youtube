using Microsoft.AspNetCore.Identity;

namespace AspNetYoutube.Models;

public class User : IdentityUser
{
    public ICollection<Video>? Videos { get; set; }
}