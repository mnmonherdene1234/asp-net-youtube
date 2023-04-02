using AspNetYoutube.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("Database") ?? "";
builder.Services.AddDbContext<DatabaseContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DatabaseContext>();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login";
});

var app = builder.Build();

app.MapRazorPages();
app.UseStaticFiles();

app.Run();
