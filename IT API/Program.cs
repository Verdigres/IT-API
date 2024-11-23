using Microsoft.EntityFrameworkCore;
using ProductApi.Data; // Upewnij się, że ta przestrzeń nazw jest dodana

var builder = WebApplication.CreateBuilder(args);

// Dodaj usługi do kontenera
builder.Services.AddControllers();

// Konfiguruj DbContext
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));

// Dodaj pozostałe usługi (np. CORS, Swagger)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// **Inicjalizacja bazy danych**
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ProductContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        // Loguj wyjątek (możesz użyć narzędzia do logowania)
        Console.WriteLine($"Błąd podczas inicjalizacji bazy danych: {ex.Message}");
    }
}

// Konfiguracja potoku żądań HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
