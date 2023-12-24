using System.Net;
using System.Security.Claims;
using lanstreamer_api.App.Data.Models.Enums;
using lanstreamer_api.App.Exceptions;

namespace lanstreamer_api.App.Attributes;

public class AuthorizationAttribute : Attribute
{
    private readonly Role _role;

    public AuthorizationAttribute(Role role = Role.User)
    {
        _role = role;
    }

    public Task CheckRole(HttpContext context)
    {
        var user = context.User;

        var userRoles = GetCurrentUserRoles(user);

        if (!userRoles.Contains(_role))
        {
            throw new AppException(HttpStatusCode.Unauthorized, "User doesn't have required role");
        }

        return Task.CompletedTask;
    }

    private List<Role> GetCurrentUserRoles(ClaimsPrincipal user)
    {
        var userClaims = user.Claims;
        var roles = userClaims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        
        return roles.Select(r => Enum.Parse<Role>(r)).ToList();
    }
}