using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AspNetYoutube.Models;

namespace AspNetYoutube.Pages;

public class LoginModel : PageModel
{
    public SignInManager<User> SignInManager { get; set; }

    public LoginModel(SignInManager<User> signInManager)
    {
        SignInManager = signInManager;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync(string email, string password)
    {
        var result = await SignInManager.PasswordSignInAsync(email, password, false, false);

        if (result.Succeeded)
        {
            return new JsonResult(new
            {
                message = "success"
            });
        }

        return new JsonResult(new
        {
            message = "error"
        });
    }
}