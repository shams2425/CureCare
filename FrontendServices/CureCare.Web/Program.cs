var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Cookie authentication: MVC login state maintain karega.
builder.Services
    .AddAuthentication("CureCareCookie")
    .AddCookie("CureCareCookie", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";

        options.Cookie.Name = "CureCare.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy =
            Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        options.Cookie.SameSite =
            Microsoft.AspNetCore.Http.SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// PatientService API connection
builder.Services.AddHttpClient("PatientApi", client =>
{
    var baseUrl = builder.Configuration["PatientApi:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException(
            "PatientApi:BaseUrl is missing from appsettings.json.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

// AuthService API connection
builder.Services.AddHttpClient("AuthApi", client =>
{
    var baseUrl = builder.Configuration["AuthApi:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException(
            "AuthApi:BaseUrl is missing from appsettings.json.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();