using DelegatingHandlerExamples.Shared.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;

namespace DelegatingHandlerExamples.Shared.Middlewares;

public class ValidateTokenMiddleware
{
    private readonly RequestDelegate _next;

    public ValidateTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    [Obsolete]
    public async Task Invoke(HttpContext context)
    {

        if (context?.User?.Identity?.IsAuthenticated == false)
        {
            List<Claim> userClaims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, "")
            };

            var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            await context.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });
        }

        await _next.Invoke(context);
    }
}


