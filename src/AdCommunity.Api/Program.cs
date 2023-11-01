using AdCommunity.Api.Middlewares;
using AdCommunity.Core;
using AdCommunity.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Serilog Implementation
using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("./logs.txt")
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(log);
#endregion

#region CustomMapper and CustomMediator Implementation
builder.Services.AddYtMapper();
builder.Services.AddYtMeditor(AppDomain.CurrentDomain.Load("AdCommunity.Application"));
#endregion

#region Repository Registration Implementation
RepositoryServiceRegistration.AddRepositoryRegistration(builder.Services, builder.Configuration);
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Exception Middleware Implementation
app.UseMiddleware<ExceptionMiddleware>();
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
