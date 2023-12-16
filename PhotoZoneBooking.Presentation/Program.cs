using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using PhotoZoneBooking.BLL.Configs;
using PhotoZoneBooking.BLL.Interfaces;
using PhotoZoneBooking.BLL.Middlewares;
using PhotoZoneBooking.BLL.Profiles;
using PhotoZoneBooking.BLL.Services;
using PhotoZoneBooking.DAL;
using PhotoZoneBooking.DAL.Entities;
using PhotoZoneBooking.DAL.Interfaces;
using PhotoZoneBooking.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich
                                               .FromLogContext()
                                               .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//Setup configs
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddScoped<SmtpClient>(serviceProvider =>
{
    var emailConfig = builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>();

    return new SmtpClient
    {
        Host = emailConfig.Server,
        Port = emailConfig.Port,
        EnableSsl = emailConfig.EnableSsl,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(emailConfig.Email, emailConfig.Password)
    };
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IPhotoZoneService, PhotoZoneService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddDbContext<DataContext>(a =>
    a.UseNpgsql(builder.Configuration.GetSection("ConnectionString").Value,
        b => b.MigrationsAssembly("PhotoZoneBooking.Presentation")
    )
);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

builder.Services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

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
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=PhotoZone}/{action=Index}/");
    endpoints.MapRazorPages();
});

app.Run();