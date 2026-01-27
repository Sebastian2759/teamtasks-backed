namespace Api.Helpers;

public static class JwtTokenHelper
{
    public static string GetUserEmail(HttpContext context)
    {
        var claim = context?.User?.FindFirst("preferred_username");
        return claim?.Value;
    }

    public static string GetUserName(HttpContext context)
    {
        var claim = context?.User?.FindFirst("name");
        return claim?.Value;
    }
}