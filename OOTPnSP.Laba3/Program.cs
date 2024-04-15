using OOTPnSP.Laba2.Models;
using OOTPnSP.Laba2.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

ShapeService.Format = FileFormat.Xml;

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
