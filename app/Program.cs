using System.Text;
using BircheMmoUserApi.Config;
using BircheMmoUserApi.Middleware;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserViewService, UserViewService>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

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

// builder.Services.AddAuthentication();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//     options => 
//     {
//       options.TokenValidationParameters = new()
//       {
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>().Issuer,
//         ValidAudience = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>().Audience,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>().Key))
//       };
//     }
//   );

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
}

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();