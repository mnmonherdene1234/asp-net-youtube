using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using AspNetYoutube.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetYoutube.Pages;

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

    public async Task<IActionResult> OnPostAddVideo(string title, IFormFile video)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "videos", video.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await video.CopyToAsync(stream);
        }

        Video vid = new()
        {
            UserId = userId,
            Title = title,
            Url = $"/videos/{video.FileName}"
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