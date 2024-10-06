using System.Reflection;
using System.Text;
using Common.Authorization;
using Common.Settings;
using Entity.Entities;
using Infrastructure;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

[assembly: XmlConfigurator(ConfigFile = "log4net.config")]

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.Configure<StrJWT>(builder.Configuration.GetSection("StrJWT"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var strJwtSettingSection = builder.Configuration.GetSection("StrJWT");
var strJwtSettings = strJwtSettingSection.Get<StrJWT>();

// Load configuration
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

builder.Services.AddDbContext<Context>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")),
    ServiceLifetime.Transient);
builder.Services.AddTransient<DbContext, Context>();
builder.Services.RegisterInfrastructureServices(builder.Configuration);
//builder.Services.AddCors(o => o.AddPolicy("MyCors", build =>
//{
//    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
//}));
//builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = strJwtSettings.Issuer,
            ValidAudience = strJwtSettings.Audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strJwtSettings.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

// set Token on swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = " API TMS",
        Description = "ASP.NET Core 7.0 Web API",
        TermsOfService = new Uri("https://api-tms.azurewebsites.net/swagger")
    });
    // To Enable authorization using Swagger (JWT)
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    s.IncludeXmlComments(xmlPath);
});

builder.Services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

builder.Services.Configure<FormOptions>(options =>
{
    options.MemoryBufferThreshold = int.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MultipartBoundaryLengthLimit = int.MaxValue;
    options.MultipartHeadersCountLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIDocV1"); });
}
else if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocV1"); });
}

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());

// authentication Token
app.MapControllers().RequireAuthorization();

app.Run();