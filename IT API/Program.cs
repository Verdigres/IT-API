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
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://kusiubruk.42web.io",
                         "https://kusiubruk.42web.io",
                         "http://localhost:5000",
                         "http://localhost:5097")
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

//testowe połączenie
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ProductContext>();
        // Test połączenia z bazą
        if (context.Database.CanConnect())
        {
            Console.WriteLine("Połączenie z bazą danych nawiązane pomyślnie.");
        }
        else
        {
            Console.WriteLine("Błąd połączenia z bazą danych.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Błąd połączenia z bazą: {ex.Message}");
    }
}


app.UseSwagger();
app.UseSwaggerUI();

// Konfiguracja potoku żądań HTTP
//if (app.Environment.IsDevelopment())
//{
    app.UseDeveloperExceptionPage();
    
//}

app.UseRouting();

//app.UseCors("AllowAll");
app.UseCors("AllowSpecificOrigin");


app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello from Minimal API!");

app.Run();
