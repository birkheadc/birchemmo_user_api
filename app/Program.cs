using BircheMmoUserApi.Config;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserViewService, UserViewService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddSingleton(
    builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>()
  );
  builder.Services.AddSingleton(
    builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>()
  );
  Console.WriteLine(builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>().Key);
}
else
{
  builder.Services.AddSingleton(
    new EmailConfig()
  );
  builder.Services.AddSingleton(
    new JwtConfig()
  );
}


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

if (app.Environment.IsProduction())
{
  app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();