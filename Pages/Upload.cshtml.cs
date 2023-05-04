using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using AspNetYoutube.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AspNetYoutube.Pages;

[Authorize]
[IgnoreAntiforgeryToken]
public class UploadModel : PageModel
{
    public DatabaseContext DatabaseContext { get; set; }
    public UserManager<User> UserManager { get; set; }
    public IWebHostEnvironment WebHostEnvironment { get; set; }
    public UploadModel(DatabaseContext databaseContext, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
    {
        DatabaseContext = databaseContext;
        UserManager = userManager;
        WebHostEnvironment = webHostEnvironment;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAddVideo(string title, IFormFile? video, string? youtubeUrl)
    {
        var userValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userValue == null)
        {
            return new JsonResult(new
            {
                message = "USER_VALUE_NOT_FOUND"
            });
        }

        var userId = Guid.Parse(userValue);

        if (video != null)
        {
            var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "videos", video.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await video.CopyToAsync(stream);
            }
        }

        var user = await DatabaseContext.Users.FindAsync(userId.ToString());

        if (user == null)
        {
            return new JsonResult(new
            {
                message = "USER_NOT_FOUND"
            });
        }

        Video vid = new()
        {
            Title = title,
            Url = video != null ? $"/videos/{video.FileName}" : null,
            UserId = user.Id,
            YoutubeUrl = youtubeUrl
        };

        if (DatabaseContext.Videos != null)
        {
            await DatabaseContext.Videos.AddAsync(vid);
            await DatabaseContext.SaveChangesAsync();

            return new JsonResult(new { message = "success" });
        }

        return new JsonResult(new { message = title });
    }
}