using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using AspNetYoutube.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AspNetYoutube.Pages;

[Authorize]
[IgnoreAntiforgeryToken]
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

        if (DatabaseContext.Videos == null)
        {
            return new JsonResult(new
            {
                message = "VIDEOS_NOT_FOUND"
            });
        }

        var video = await DatabaseContext.Videos.FindAsync(Guid.Parse(id));

        if (video == null)
        {
            return new JsonResult(new
            {
                message = "VIDEO_NOT_FOUND"
            });
        }

        if (DatabaseContext.Users == null)
        {
            return new JsonResult(new
            {
                message = "USERS_NOT_FOUND"
            });
        }

        return new JsonResult(video);
    }

    public async Task<IActionResult> OnGetComments()
    {
        string id = Convert.ToString(Request.Query["id"]);

        if (DatabaseContext.Videos == null)
        {
            return new JsonResult(new
            {
                message = "VIDEOS_NOT_FOUND"
            });
        }

        var video = await DatabaseContext.Videos.FindAsync(new Guid(id));

        if (video == null)
        {
            return new JsonResult(new
            {
                message = "VIDEO_NOT_FOUND"
            });
        }

        if (DatabaseContext.Comments == null)
        {
            return new JsonResult(new
            {
                message = "COMMENTS_NOT_FOUND"
            });
        }

        var comments = DatabaseContext.Comments.Where((c) => c.Video == video).ToList();

        foreach (var comment in comments)
        {
            comment.User = await DatabaseContext.Users.FindAsync(comment.UserId);
        }

        return new JsonResult(comments ?? new List<Comment>());
    }

    public async Task<IActionResult> OnGetUser()
    {
        string id = Convert.ToString(Request.Query["id"]);

        if (DatabaseContext.Videos == null)
        {
            return new JsonResult(new
            {
                message = "VIDEOS_NOT_FOUND"
            });
        }



        var video = await DatabaseContext.Videos.FindAsync(new Guid(id));

        if (video == null)
        {
            return new JsonResult(new
            {
                message = "VIDEO_NOT_FOUND"
            });
        }

        if (DatabaseContext.Users == null)
        {
            return new JsonResult(new
            {
                message = "USERS_NOT_FOUND"
            });
        }

        var user = await DatabaseContext.Users.FindAsync(video.UserId);

        if (user == null)
        {
            return new JsonResult(new
            {
                message = "USER_NOT_FOUND"
            });
        }

        return new JsonResult(user);
    }

    public async Task<IActionResult> OnPostSaveComment(string comment, string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return new JsonResult(new
            {
                message = "ID_NOT_FOUND"
            });
        }

        string videoId = Convert.ToString(id);

        if (DatabaseContext.Videos == null)
        {
            return new JsonResult(new
            {
                message = "VIDEOS_NOT_FOUND"
            });
        }

        var video = await DatabaseContext.Videos.FindAsync(Guid.Parse(videoId));

        if (video == null)
        {
            return new JsonResult(new
            {
                message = "VIDEO_NOT_FOUND"
            });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return new JsonResult(new
            {
                message = "USER_NOT_FOUND"
            });
        }

        var user = await DatabaseContext.Users.FindAsync(userId);

        if (user == null)
        {
            return new JsonResult(new
            {
                message = "USER_NOT_FOUND"
            });
        }

        Comment newComment = new()
        {
            Text = comment,
            VideoId = video.Id,
            UserId = user.Id
        };

        if (DatabaseContext.Comments == null)
        {
            return new JsonResult(new
            {
                message = "COMMENTS_NOT_FOUND"
            });
        }

        await DatabaseContext.Comments.AddAsync(newComment);
        await DatabaseContext.SaveChangesAsync();

        return new JsonResult(newComment);
    }
}