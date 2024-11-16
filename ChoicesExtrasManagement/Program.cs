using ChoicesExtrasManagement.Data;
using ChoicesExtrasManagement.Interfaces;
using ChoicesExtrasManagement.Models;
using ChoicesExtrasManagement.Repository;
using ChoicesExtrasManagement.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Stripe;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Map Interfaces with Repository
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IPlotRepository, PlotRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IPlotTypeRepository, PlotTypeRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IPlotTypeRoomMappingRepository, PlotTypeRoomMappingRepository>();
builder.Services.AddScoped<ICatalogueRepository, CatalogueRepository>();
builder.Services.AddScoped<ISubCatalogueRepository, SubCatalogueRepository>();
builder.Services.AddScoped<ICatalogueRoomPlotTypeMappingRepository, CatalogueRoomPlotTypeMappingRepository>();
builder.Services.AddScoped<ISavedChoiceRepository, SavedChoiceRepository>();
builder.Services.AddScoped<ISavedExtraRepository, SavedExtraRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddDbContext<ChoicesExtrasManagementDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ChoicesExtrasManagementDbContext>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//        policy =>
//    {        
//        policy.WithOrigins("https://localhost:7158").AllowAnyHeader().AllowAnyMethod();
//    });
//});

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "setinitialdata")
{
    //await Seed.SeedUsersAndRolesAsync(app);
    Seed.SeedData(app);
}

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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseCors(MyAllowSpecificOrigins);

app.Run();


