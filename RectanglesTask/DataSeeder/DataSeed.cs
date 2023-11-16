using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RectanglesTask.Models;
using RectanglesTask.Models.DAL;
using System.Drawing;

namespace RectanglesTask.DataSeeder
{
    public class DataSeed
    {
        private readonly IServiceProvider _serviceProvider;
        public DataSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void AddRectanglesData(RectangleContext context)
        {
            if (!context.Rectangles.Any())
            {
                Random random = new Random();

                for (int i = 0; i < 200; i++)
                {
                    var rectangle = new Models.Rectangle
                    {
                        X = random.Next(0, 100), // Random X coordinate between 0 and 100
                        Y = random.Next(0, 100), // Random Y coordinate between 0 and 100
                        Width = random.Next(1, 20), // Random width between 1 and 20
                        Height = random.Next(1, 20) // Random height between 1 and 20
                    };
                    context.Rectangles.Add(rectangle);
                }

                context.SaveChanges();
            }


        }

        public void AddTempuserData(RectangleContext context)
        {
            if (!context.Users.Any())
            {

                ApplicationUser user = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = "testuser@gmail.com",
                };


                var userManager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                userManager.CreateAsync(user, "Rectangle123@").Wait();
            }
        }
    }
}
