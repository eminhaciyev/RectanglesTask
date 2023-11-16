using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RectanglesTask.Models.DAL
{
    public class RectangleContext : IdentityDbContext<ApplicationUser>
    {
        public RectangleContext(DbContextOptions<RectangleContext> options) : base(options) { }

        public DbSet<Rectangle> Rectangles { get; set; }
    }
}
