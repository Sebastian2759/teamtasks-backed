using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace Api.Helpers;

public static class SimulatedUserHelper
{
    public static void UseSimulatedUser(TokenValidatedContext context, IConfigurationManager configuration)
    {
        if (bool.Parse(configuration.GetSection("SimulatedUser")["IsSimulated"]))
        {
            // Realiza modificaciones en los claims para simular el usuario
            var claims = new List<Claim>
            {
                new("name", "User Simulated"),
                new("preferred_username", configuration.GetSection("SimulatedUser")["UserEmail"].ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims, context.Principal.Identity.AuthenticationType, ClaimTypes.Name, ClaimTypes.Role);
            context.Principal = new ClaimsPrincipal(claimsIdentity);
        }
    }
}