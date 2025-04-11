using best_hackathon_2025.MongoDB;
using best_hackathon_2025.Repositories.Implementations;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.Settings;   
using best_hackathon_2025.Services;   
using best_hackathon_2025.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Inclusive Map API", Version = "v1" });
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.Configure<GoogleMapsOptions>(
    builder.Configuration.GetSection("GoogleMaps"));
builder.Services.AddHttpClient<GoogleMapsService>();


builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<JwtTokenGenerator>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPointRepository, PointRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();
builder.Services.AddScoped<IPointRequestRepository, PointRequestRepository>();

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
        Console.WriteLine("### JWT CONFIG LOADED ###");
        Console.WriteLine($"Issuer   : {jwt.Issuer}");
        Console.WriteLine($"Audience : {jwt.Audience}");
        Console.WriteLine($"Secret   : {jwt.Secret} (len {jwt.Secret.Length})");
        Console.WriteLine("#########################");

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                                   Encoding.UTF8.GetBytes(jwt.Secret))
        };
    });



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inclusive Map API v1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();          

app.Run();
