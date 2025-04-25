using QuizApp.Service;

var builder = WebApplication.CreateBuilder(args);

// Lägg till MVC-tjänster
builder.Services.AddControllersWithViews();

// Lägg till session (kom ihåg att lägga session i programmet)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Registrera HttpClient och vår QuizService
builder.Services.AddHttpClient<QuizService>();

var app = builder.Build();

// Konfigurera middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Aktivera session innan autorisering
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Index}/{id?}");

app.Run();
