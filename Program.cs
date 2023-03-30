using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ljcProject5.Data;
using ljcProject5.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ljcProject5ContextConnection") ?? throw new InvalidOperationException("Connection string 'ljcProject5ContextConnection' not found.");

builder.Services.AddDbContext<ljcProject5Context>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(10,4,27))));

builder.Services.AddDefaultIdentity<ljcProject5User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ljcProject5Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
