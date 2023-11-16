using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RectanglesTask.Models;
using RectanglesTask.Models.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using RectanglesTask.Services.UserInterface;
using RectanglesTask.Services.UserService;
using RectanglesTask.Services.Auth;
using Microsoft.OpenApi.Models;
using RectanglesTask.DataSeeder;

namespace RectanglesTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Configure Swagger to include basic authentication
                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                // Add the requirement for authorization in Swagger UI
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "basic"
                    }
                },
                new string[] {}
            }
        });
            });





            // Inside ConfigureServices method in Startup.cs
            builder.Services.AddDbContext<RectangleContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<RectangleContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


            builder.Services.AddScoped<IUserService, UserService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RectangleContext>();
                DataSeed seed = new DataSeed(scope.ServiceProvider);


                seed.AddRectanglesData(dbContext);
                seed.AddTempuserData(dbContext);

            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}