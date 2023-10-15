using DelegatingHandlerExamples.Shared.TokenHelpers;
using DelegatingHandlerExamples.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace DelegatingHandlerExamples.Web.Pages;

public class LoginModel : PageModel
{
    private readonly ITokenProvider tokenProvider;

    public LoginModel(ITokenProvider tokenProvider)
    {
        this.tokenProvider = tokenProvider;
    }

    [BindProperty]
    public required InputModel Input { get; set; }

    public async Task OnPost()
    {
        if (ModelState.IsValid)
        {
            await SignInAsync();
            if (HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                RedirectToPage("/Home");
            }
        }
    }

    private async Task SignInAsync()
    {
        if (Input.Username is null || Input.Password is null)
        {
            throw new InvalidOperationException();
        }

        var token = await tokenProvider.SignInAsync(Input.Username, Input.Password);

        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, Input.Username),
            new Claim(nameof(token.AccessToken), token.AccessToken),
            new Claim(nameof(token.RefreshToken), token.RefreshToken)
        };

        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = true
            });
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return RedirectToPage("/Index");
    }
}

public class InputModel
{
    [Required]
    [Display(Name = "User Name")]
    public string? Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; }
}
