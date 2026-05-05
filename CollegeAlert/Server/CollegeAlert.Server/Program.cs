using CollegeAlert.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Use port 5050 to avoid conflict with macOS AirPlay on port 5000
builder.WebHost.UseUrls("http://0.0.0.0:5050");
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss.fff ";
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("CollegeAlert.Server");
logger.LogInformation("server starting on http://0.0.0.0:5050");

// app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<AlertHub>("/alerthub");

app.Run();
