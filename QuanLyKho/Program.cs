using Microsoft.EntityFrameworkCore;
using QuanLyKho.Models.EF;
using Microsoft.AspNetCore.Identity;
using QuanLyKho.Models.Entities;
using QuanLyKho.Models;
using System.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using QuanLyKho.Services.Implement;
using QuanLyKho.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Routing;
using DinkToPdf.Contracts;
using DinkToPdf;
using AutoMapper;
using QuanLyKho.Extensions;
using Hangfire;
using Hangfire.SqlServer;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("QLKHO");
    options.UseSqlServer(connectionString);

    //var mySql_ConnectionString = builder.Configuration.GetConnectionString("QLKHO_MYSQL");
    //options.UseMySQL(mySql_ConnectionString);
});

builder.Services.AddOptions();

// Mail config
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddSingleton<IEmailSender, SendMailService>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


builder.Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/forbidden.html";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
});


builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Khôg bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 0; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); // Khóa 5 phút
    //options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = false;          // Yêu cầu xác thực tài khoản mới được đăng nhập
});


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<PromotionService>();


// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("QLKHO")));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();


// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson();
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

app.UseHangfireDashboard();


var backgroundJobs = app.Services.GetService<IBackgroundJobClient>();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    var promotionService = services.GetRequiredService<PromotionService>();

    RecurringJob.AddOrUpdate("UpdatePromotionPriceStartDate", () => promotionService.UpdatePromotionPriceStartDate(), Cron.Daily);

    RecurringJob.AddOrUpdate("UpdatePromotionPriceEndDate", () => promotionService.UpdatePromotionPriceEndDate(), Cron.Daily);
}



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
