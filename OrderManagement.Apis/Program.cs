
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Apis.Errors;
using OrderManagement.Apis.Extensions;
using OrderManagement.Apis.Middlewares;
using OrderManagement.Repository.Data;
using OrderManagement.Repository.Identity;
using StackExchange.Redis;

namespace OrderManagement.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
            );
            builder.Services.AddSingleton<IConnectionMultiplexer>(options => {

                var connection = builder.Configuration.GetConnectionString("RedisConnection");

                return ConnectionMultiplexer.Connect(connection);
            });
            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion
            var app = builder.Build();

            #region Update Database & Create Roles & Create UserAdmin
            var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();
            var factory = services.GetRequiredService<ILoggerFactory>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            try
            {
                await CreateRolesAsync(roleManager, userManager);
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                var logger = factory.CreateLogger<Program>();
                logger.LogError(e, "there is an problem while updating Database");
            }

            // Create the admin user if it doesn't exist
            var adminUser = await userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            #endregion

            // Configure the HTTP request pipeline.
            #region Add Middlewares
            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
            #endregion
            app.Run();
            async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
           {
            string[] roleNames = { "Admin", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }
        }
    }
}
