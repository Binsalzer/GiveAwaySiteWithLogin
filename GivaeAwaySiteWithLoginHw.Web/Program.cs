namespace GiveAwaySiteWithLoginHw.Web
{
    public class Program
    {
        private static string CookieScheme = "GiveAwaySiteWithLoginHw";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddAuthentication(CookieScheme)
           .AddCookie(CookieScheme, options =>
           {
               options.LoginPath = "/account/login";
           });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}