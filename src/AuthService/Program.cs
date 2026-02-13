using AuthService.Data;
using AuthService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Security.Extensions;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

//-----------------------------------------------------------------------
// => Register DbContext, the <TContext> is AuthDbContext
//-----------------------------------------------------------------------
builder.Services.AddDbContext<AuthDbContext>(builder.Configuration, "AuthDb");
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// => Register Identity service + Configure Jwt Tokan
//-----------------------------------------------------------------------
builder.Services.AddIdentityServices<AppUser, AppRole, AuthDbContext>(builder.Configuration);
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// => Register AutoMapper
//-----------------------------------------------------------------------
builder.Services.AddAutoMapper(typeof(Program));
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// => Register CORS Services
//-----------------------------------------------------------------------
builder.Services.AddCorsServices(builder.Configuration);
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// => Register Controllers
//-----------------------------------------------------------------------
builder.Services.AddControllers();
//-----------------------------------------------------------------------

//-----------------------------------------------------------------------
// => Register Health Checks
//-----------------------------------------------------------------------
builder.Services.AddHealthChecks()
   .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" });
//-----------------------------------------------------------------------

////////////////////////////////////////////////////////////////////////////
////////////////////////////// Hosted Services /////////////////////////////
////////////////////////////////////////////////////////////////////////////

// => Register Database Initial Operations
builder.Services.AddHostedService<AuthDbHostedService>();

////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////
//////////////////////////////// Versioning ////////////////////////////////
////////////////////////////////////////////////////////////////////////////
builder.Services.AddApiVersioning(options =>
{
   options.ReportApiVersions = true;
   options.AssumeDefaultVersionWhenUnspecified = true;
   options.DefaultApiVersion = new ApiVersion(1, 0);
   options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
   options.GroupNameFormat = "'v'VVV";
   options.SubstituteApiVersionInUrl = true;
});
////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////

//-----------------------------------------------------------------------
// => Register Swagger + Jwt
//-----------------------------------------------------------------------
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
   opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
      Name = "Authorization",
      Type = SecuritySchemeType.ApiKey,
      Scheme = "Bearer",
      BearerFormat = "JWT",
      In = ParameterLocation.Header,
      Description = "Enter: Bearer {your token}"
   });

   opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});
//-----------------------------------------------------------------------

var app = builder.Build();

//-----------------------------------------------------------------------
// => Run Swagger in development only!
//-----------------------------------------------------------------------
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//    app.UseSwagger();
//    app.UseSwaggerUI();
// }
//-----------------------------------------------------------------------

app.UseSwagger();
app.UseSwaggerUI();

// => Configur pipeline
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Configure Health Check Endpoint
app.MapHealthChecks("/health");

app.Run();

//-----------------------------------------------------------------------
// => Make the implicit Program class public so test projects can access it
//-----------------------------------------------------------------------
public partial class Program { }
//-----------------------------------------------------------------------
