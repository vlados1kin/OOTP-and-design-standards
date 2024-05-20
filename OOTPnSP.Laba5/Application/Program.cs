using Application.Services;
using PluginBase;

var builder = WebApplication.CreateBuilder(args);

FigureService.Format = SerializationFormat.Json;
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();