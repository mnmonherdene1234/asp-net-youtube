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
    public IWebHostEnvironment WebHostEnvironment { get; set; }

    public VideosModel(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment)
    {
        DatabaseContext = databaseContext;
        WebHostEnvironment = webHostEnvironment;
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

    public async Task<IActionResult> OnPostDelete(string id)
    {
        if (DatabaseContext.Videos == null)
        {
            return new JsonResult(new
            {
                message = "VIDEOS_NOT_FOUND"
            });
        }

        var video = await DatabaseContext.Videos.FindAsync(Guid.Parse(id));

        if (video != null)
        {
            DatabaseContext.Videos.Remove(video);

            if (video.Url != null)
            {
                string filePath = Path.Join(WebHostEnvironment.ContentRootPath, "wwwroot", video.Url);
                if (System.IO.File.Exists(filePath)) // Check if the file exists
                {
                    System.IO.File.Delete(filePath); // Delete the file
                }

            }
        }


        int result = await DatabaseContext.SaveChangesAsync();

        if (result != 1)
        {
            return new JsonResult(new
            {
                message = "ERROR"
            });
        }

        return new JsonResult(new
        {
            message = "OK"
        });
    }

    public async Task<IActionResult> OnPostChangeTitle(string id, string title)
    {
        if (DatabaseContext.Videos == null)
        {
            return new JsonResult(new
            {
                message = "VIDEOS_NOT_FOUND"
            });
        }

        var video = await DatabaseContext.Videos.FindAsync(Guid.Parse(id));

        if (video != null)
        {
            video.Title = title;
        }


        int result = await DatabaseContext.SaveChangesAsync();

        if (result != 1)
        {
            return new JsonResult(new
            {
                message = "ERROR"
            });
        }

        return new JsonResult(new
        {
            message = "OK"
        });
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

    public async Task<IActionResult> OnGetAuthor()
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

    public async Task<IActionResult> OnGetUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return new JsonResult(new
            {
                message = "USER_VALUE_NOT_FOUND"
            });
        }

        if (DatabaseContext.Users == null)
        {
            return new JsonResult(new
            {
                message = "USERS_NOT_FOUND"
            });
        }

        var user = await DatabaseContext.Users.FindAsync(userId);

        return new JsonResult(user);
    }
}