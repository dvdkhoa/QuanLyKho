using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.EF;
using Microsoft.AspNetCore.Identity;
using QuanLyKho.Models.Entities;
using QuanLyKho.Models;
using System.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using QuanLyKho.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>( options =>
{
    var connectionString = builder.Configuration.GetConnectionString("QLKHO");
    options.UseSqlServer(connectionString);
});

builder.Services.AddOptions();

// Mail config
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton<IEmailSender, SendMailService>();

builder.Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/forbidden.html";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
