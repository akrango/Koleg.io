using System;
using System.Linq;
using System.Security.Claims;
namespace Koleg.io.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetFirstName(this ClaimsPrincipal principal)
        {
            var firstName = principal.Claims.FirstOrDefault(c => c.Type == "FirstName");
            return firstName?.Value;
        }
    }
}
