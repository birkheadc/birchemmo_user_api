using BircheMmoUserApi.Config;
using BircheMmoUserApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IEmailService, EmailService>();

if (builder.Environment.IsDevelopment())
{
  builder.Services.AddSingleton(
    builder.Configuration.GetSection("EmailConfigNoReply").Get<EmailConfig>()
  );
  builder.Services.AddSingleton(
    builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>()
  );
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
