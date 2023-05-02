using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using AspNetYoutube.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.Pages;

[Authorize]
public class IndexModel : PageModel
{
    public UserManager<User> UserManager { get; set; }
    public SignInManager<User> SignInManager { get; set; }
    public DatabaseContext DatabaseContext { get; set; }

    public IndexModel(DatabaseContext databaseContext, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        DatabaseContext = databaseContext;
        UserManager = userManager;
        SignInManager = signInManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetUser()
    {
        var user = await UserManager.GetUserAsync(HttpContext.User);

        if (user == null)
        {
            return new JsonResult(new
            {
                message = "USER_NOT_FOUND"
            });
        }

        return new JsonResult(user);
    }

    public async Task<IActionResult> OnGetLogOutAsync()
    {
        await SignInManager.SignOutAsync();

        return new JsonResult(new
        {
            message = "OK"
        });
    }

    public async Task<IActionResult> OnGetVideos()
    {
        string q = Convert.ToString(Request.Query["q"]);

        var videos = DatabaseContext.Videos.Where(v => v.Title.ToUpper().Contains((q ?? "").ToUpper())).ToList();

        foreach (var video in videos)
        {
            video.User = await DatabaseContext.Users.FindAsync(video.UserId.ToString());
        }

        return new JsonResult(videos);
    }
}