using Microsoft.AspNetCore.Identity;

namespace RectanglesTask.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Password { get; set; }
    }
}
