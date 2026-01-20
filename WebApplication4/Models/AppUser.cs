using Microsoft.AspNetCore.Identity;

namespace WebApplication4.Models;

public class AppUser:IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
