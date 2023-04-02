using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AspNetYoutube.Models;

namespace AspNetYoutube.Pages;

public class RegisterModel : PageModel
{
    public SignInManager<User> SignInManager { get; set; }
    public UserManager<User> UserManager { get; set; }

    public RegisterModel(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        SignInManager = signInManager;
        UserManager = userManager;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        string email = Request.Form["email"].ToString();
        string password = Request.Form["password"].ToString();

        var user = new User
        {
            UserName = email,
            Email = email
        };

        var result = await UserManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await SignInManager.SignInAsync(user, false);
            return new JsonResult(user);
        }

        return new JsonResult(new { message = "error" });
    }
}