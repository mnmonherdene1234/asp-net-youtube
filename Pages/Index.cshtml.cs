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

    public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
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
}