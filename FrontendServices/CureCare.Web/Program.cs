
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
