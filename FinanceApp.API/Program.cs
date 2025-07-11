using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Http.Features;

using FinanceApp.API.Middleware;
using FinanceApp.API.Swagger;
using FinanceApp.API.Extensions;
using FinanceApp.Infrastructure;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Extensions;
using FinanceApp.Shared.Helper;
using FinanceApp.API.Filters;
using FinanceApp.JWTAuthenticationHandler;
using Microsoft.AspNetCore.HttpOverrides;




var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------
// 1. Configuration
// ----------------------------------------------------------------
builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

var configuration = builder.Configuration;

// ----------------------------------------------------------------
// 2. Serilog Logging
// ----------------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// ----------------------------------------------------------------
// 3. Dependency Injection
// ----------------------------------------------------------------
builder.Services.AddOptions()
    .Configure<AppConfiguration>(configuration)
    .AddTransient<IConfiguration>(_ => configuration);

builder.Services.AddInfrastructureServices();
builder.Services.AddCustomRateLimiting(configuration);

// ----------------------------------------------------------------
// 4. FluentValidation + Custom Error Formatting
// ----------------------------------------------------------------
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Continue;
ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidationServices();
builder.Services.ConfigureCustomModelValidation();
builder.Services.JWTConfigValidator(configuration);
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
// ----------------------------------------------------------------
// 5. Controllers + Filters + JSON Settings
// ----------------------------------------------------------------
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilterAttribute));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.WriteIndented = true;
});

// ----------------------------------------------------------------
// 6. Swagger + API Versioning
// ----------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new MediaTypeApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// ----------------------------------------------------------------
// 7. Compression (for response size reduction)
// ----------------------------------------------------------------
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.SmallestSize);
builder.Services.Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.SmallestSize);

// ----------------------------------------------------------------
// 8. Form Request Size Configuration
// ----------------------------------------------------------------
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBoundaryLengthLimit = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MemoryBufferThreshold = 1024000000;
});

// ----------------------------------------------------------------
// 9. Database Setup
// ----------------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DBConfiguration")));

// ----------------------------------------------------------------
// 10. CORS Policies
// ----------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());

    options.AddPolicy("ProdCors", policy =>
        policy.WithOrigins("https://yourfrontenddomain.com")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ----------------------------------------------------------------
// Build the App
// ----------------------------------------------------------------
var app = builder.Build();

// Forwarded headers (if behind reverse proxy)
var forwardOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
};
forwardOptions.KnownNetworks.Clear();
forwardOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardOptions);

// Logging and middleware
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseStaticFiles();
app.UseRouting();  // 👈 Routing must be before CORS, Auth

// CORS — based on environment
if (app.Environment.IsDevelopment())
    app.UseCors("DevCors");
else
    app.UseCors("ProdCors");

// ✅ Authentication must come BEFORE MapControllers
app.UseAuthentication();   // 🔐 Validates JWT
app.UseAuthorization();    // 🔐 Enforces [Authorize] attribute

// Optional
app.UseResponseCompression();
app.UseRateLimiter();

// ✅ Map controllers AFTER auth
app.MapControllers();

// Swagger
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            $"FinanceApp API {description.GroupName.ToUpperInvariant()}"
        );
    }
    options.RoutePrefix = string.Empty;
});

// Run app
try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed.");
}
finally
{
    Log.CloseAndFlush();
}
