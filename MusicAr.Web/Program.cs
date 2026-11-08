using Microsoft.AspNetCore.Authentication.Cookies;
using MusicAr.Web.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
/*
builder.Services.AddHttpClient("ApiGatewayClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5086/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});*/
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
                 ?? "http://localhost:5086/";

builder.Services.AddHttpClient("ApiGatewayClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

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

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();




