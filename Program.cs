using StudentClient.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("StudentApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5062/");
});
builder.Services.AddScoped<IStudentApiService, StudentApiService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();
