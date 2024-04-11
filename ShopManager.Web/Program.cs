using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopManager.DAL.Entities.User;
using ShopManager.Web.Data;
using ShopManager.Services.Abstract;
using ShopManager.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace ShopManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connectionStringApp = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionStringApp));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddTransient<IManagerService, EfManagerService>(provider =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

                return new EfManagerService(
                    provider.GetRequiredService<ApplicationDbContext>(),
                    provider.GetRequiredService<UserManager<UserModel>>(),
                    provider.GetRequiredService<RoleManager<IdentityRole>>());
            });


            builder.Services.AddDefaultIdentity<UserModel>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpClient();


            var app = builder.Build();

            CreateAdminRole(app).GetAwaiter().GetResult();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "/{controller=Home}/{action=index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        private static async Task CreateAdminRole(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Проверяем, существует ли роль "Admin"
            var adminRoleExists = await roleManager.RoleExistsAsync("Admin");

            // Если роль "Admin" не существует, создаем её
            if (!adminRoleExists)
            {
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
