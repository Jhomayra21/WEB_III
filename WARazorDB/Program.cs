using Microsoft.EntityFrameworkCore;
using WARazorDB.Data;
using WARazorDB.Interfaces;
using WARazorDB.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<WARazorDB.Data.TareaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el servicio de inicialización de la base de datos
// Seeder debe ser una clase normal que implementa IDbInitializer
builder.Services.AddScoped<IDbInitializer, TareaSeeder>();

var app = builder.Build();

// Llama al método de inicialización de la base de datos
SeedDatabase();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        initializer.Initialize(scope.ServiceProvider);
    }
}