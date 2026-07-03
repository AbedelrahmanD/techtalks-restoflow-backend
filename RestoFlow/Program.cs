using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestoFlow.Data;
using RestoFlow.Services;
using RestoFlow.Services.Interfaces;
using RestoFlow.Services.Implementations;
using RestoFlow.OpenApi;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text;
using System.Linq;
using RestoFlow;

var builder = WebApplication.CreateBuilder(args);

// Localization: point to Resources folder
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IDiningTableService, DiningTableService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFeedbackQuestionService, FeedbackQuestionService>();
builder.Services.AddScoped<IRefreshTokenService, RestoFlow.Services.Implementations.RefreshTokenService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
    };
});



builder.Services.AddAuthorization();
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)));
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure supported cultures and request localization
var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ar") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.ToList(),
    SupportedUICultures = supportedCultures.ToList()
};

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
