using BircheMmoUserApi.Config;
using BircheMmoUserApi.Filters;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserViewService, UserViewService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
}
else
{
  builder.Services.AddSingleton<IUserRepository, MongoDbUserRepository>();
}

builder.Services.AddHttpContextAccessor();

// Build Configs from appsettings / environment variables

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton(
  builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>()
);
builder.Services.AddSingleton(
  builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>()
);
builder.Services.AddSingleton(
  builder.Configuration.GetSection("DbConfig").Get<DbConfig>()
);
builder.Services.AddSingleton(
  builder.Configuration.GetSection("EmailVerificationConfig").Get<EmailVerificationConfig>()
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "All",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("All");
}

if (app.Environment.IsStaging())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseCors("All");
}

if (app.Environment.IsProduction())
{
  app.UseHttpsRedirection();
  app.UseCors("All");
}

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }