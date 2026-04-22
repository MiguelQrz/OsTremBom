using Microsoft.EntityFrameworkCore;
using TremBomApi.Data;              

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
// Config do Banco de dados SQLite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=TremBom.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDirectoryBrowser();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowFrontend");

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();