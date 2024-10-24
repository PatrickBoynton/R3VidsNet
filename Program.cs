using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using R3vids.Data;
using R3vids.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VideoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

app.UseStaticFiles();

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/wwwroot"
});

app.UseCors("AllowAll");

app.MapControllers();

try
{
    await Builders.InitDatabase(app);
    var timer = ScheduledInit();
}
catch (Exception e)
{
    Console.WriteLine($"----> An error occured during database initialization: {e.Message}");
    throw;
}

app.Run();
return;

Timer ScheduledInit()
{
    Console.WriteLine("----> Database initialization started.");
    return new Timer(async _ => await Builders.InitDatabase(app), null, TimeSpan.Zero, TimeSpan.FromHours(1));
}