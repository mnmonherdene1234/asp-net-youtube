using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using AspNetYoutube.Models;

namespace AspNetYoutube.Pages;

public class VideosModel : PageModel
{
    public DatabaseContext DatabaseContext { get; set; }

    public VideosModel(DatabaseContext databaseContext)
    {
        DatabaseContext = databaseContext;
    }

    public void OnGet() { }

    public async Task<IActionResult> OnGetVideo()
    {
        string id = Convert.ToString(Request.Query["id"]);

        var video = await DatabaseContext.Videos.FindAsync(new Guid(id));

        video.User = await DatabaseContext.Users.FindAsync(video.UserId.ToString());
        video.Comments = DatabaseContext.Comments.Where(c => c.VideoId == video.Id).ToList();

        return new JsonResult(video);
    }
}