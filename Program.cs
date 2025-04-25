using QuizApp.Service;

var builder = WebApplication.CreateBuilder(args);

// L�gg till MVC-tj�nster
builder.Services.AddControllersWithViews();

// L�gg till session (kom ih�g att l�gga session i programmet)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// Registrera HttpClient och v�r QuizService
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
