using AssistantApplication;
using AssistantApplication.Data;
using AssistantApplication.Repositories.Implementations;
using AssistantApplication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog.Web;


var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var angularOriginName = "AngularOrigin";
var angularOrigin = builder.Configuration.GetValue<string>("FrontAppUrl")
        ?? throw new InvalidOperationException("Connection string"
        + "'FrontAppUrl' not found.");

builder.Services.AddCors(options =>
{
    options.AddPolicy(angularOriginName,
        builder =>
        {
            builder.WithOrigins(angularOrigin)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
                   
        });
});

builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IAnswearGenerator, LoremAnswearGenerator>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(angularOriginName);

app.UseAuthorization();

app.MapControllers();

app.Run();
