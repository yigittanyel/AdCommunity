using AdCommunity.Api.Middlewares;
using AdCommunity.Application;
using AdCommunity.Core;
using AdCommunity.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Exception Middleware Implementation
builder.Services.AddTransient<ExceptionMiddleware>();
#endregion

#region Serilog Implementation
using var log = new LoggerConfiguration()
    .WriteTo.File("./logs.txt")
    .CreateLogger();

builder.Services.AddSingleton<Serilog.ILogger>(log);
#endregion

#region CustomMapper and CustomMediator Implementation
builder.Services.AddYtMapper();
builder.Services.AddYtMeditor(AppDomain.CurrentDomain.GetAssemblies());
#endregion

#region Repository Registration Implementation
RepositoryServiceRegistration.AddRepositoryRegistration(builder.Services, builder.Configuration);
#endregion

#region Application Registration Implementation
ApplicationServiceRegistration.AddApplicationRegistration(builder.Services, builder.Configuration);
#endregion

#region JWT Implementation
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#region Exception Middleware Implementation
app.UseMiddleware<ExceptionMiddleware>();
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
