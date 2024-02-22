//using AutoMapper;
//using EmployeeDAA.Api.Extensions;
//using EmployeeDAA.Api.InfraStructure;
//using EmployeeDAA.Data;
//using FluentValidation.AspNetCore;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Http.Features;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using Microsoft.SqlServer.Management.Smo.Wmi;
//using System.Text;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);
//ConfigurationManager Configuration = builder.Configuration;

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.Configure<FormOptions>(o =>
//{
//    o.ValueLengthLimit = int.MaxValue;
//    o.MultipartBodyLengthLimit = long.MaxValue;
//    o.MemoryBufferThreshold = int.MaxValue;
//});
//EmployeeDAA.Data.DataSettingsManager.DataSettings DBSettings = new();
//builder.Configuration.GetSection("DBConnectionStrings").Bind(DBSettings);

//DataSettingsManager.IntiDatabaseSettings(builder.Services, DBSettings);

//DependencyRegistrar.Register(builder.Services);

//builder.Services.AddMvc(opt =>
//{
//    opt.EnableEndpointRouting = false;
//    opt.Filters.Add(typeof(ValidateModelStateAttribute));
//}).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Program>());

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});
//builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection("AuthenticationSettings"));
//builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("PasswordEncrypt"));
//builder.Services.Configure<IISServerOptions>(options =>
//{
//    options.AllowSynchronousIO = true;
//});

//var mapperConfiguration = new MapperConfiguration(configure => configure.AddProfile<ApplicationMappingProfile>());
//mapperConfiguration.CreateMapper().InitializeMapper();
//builder.Services.AddMvc(options => { options.Filters.Add(new ErrorHandlingFilter()); }).AddJsonOptions(jsonOptions =>
//{
//    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
//    jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});
//AuthenticationSettings config = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();
//byte[] secretKey = Encoding.ASCII.GetBytes(config.SecretKey);

//builder.Services.AddAuthentication(auth =>
//{
//    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata = false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee.API", Version = "v1" });
//    c.MapType<DateTime>(() => new OpenApiSchema { Format = "dd/MMM/yyyy hh:mm tt", Type = "DateTime" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Name = "JWT Authorization header using the Bearer scheme.",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//                {
//                    {
//                          new OpenApiSecurityScheme
//                            {
//                                Reference = new OpenApiReference
//                                {
//                                    Type = ReferenceType.SecurityScheme,
//                                    Id = "Bearer"
//                                }
//                            },
//                            Array.Empty<string>()

//                    }
//                });
//});

//builder.Services.AddSwaggerGen();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "mypolicy",
//                      builder =>
//                      {
//                          builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//                      });
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.MapControllers();
//DataSettingsManager.ApplyUpMigrations(builder.Services.BuildServiceProvider());
//app.UseAuthentication();

//app.UseRouting();
//app.UseCors("mypolicy");



//app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});


//app.Run();



using AutoMapper;
using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Data;
using FluentValidation.AspNetCore;
using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.InfraStructure;
using EmployeeDAA.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;
using static EmployeeDAA.Data.DataSettingsManager;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = long.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
DataSettings DBSettings = new();
builder.Configuration.GetSection("DBConnectionStrings").Bind(DBSettings);

DataSettingsManager.IntiDatabaseSettings(builder.Services, DBSettings);

DependencyRegistrar.Register(builder.Services);

builder.Services.AddMvc(opt =>
{
    opt.EnableEndpointRouting = false;
    opt.Filters.Add(typeof(ValidateModelStateAttribute));
}).AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection("AuthenticationSettings"));
builder.Services.Configure<EncryptionSettings>(builder.Configuration.GetSection("PasswordEncrypt"));
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
var mapperConfiguration = new MapperConfiguration(configure => configure.AddProfile<ApplicationMappingProfile>());
mapperConfiguration.CreateMapper().InitializeMapper();
builder.Services.AddMvc().AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
    jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//builder.Services.AddApiVersioning(config =>
//{
//    config.DefaultApiVersion = new(1, 0);
//    config.AssumeDefaultVersionWhenUnspecified = true;
//    config.ReportApiVersions = true;
//    config.ApiVersionReader = new HeaderApiVersionReader("api-versiApiVersionon");
//});
AuthenticationSettings config = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();
byte[] secretKey = Encoding.ASCII.GetBytes(config.SecretKey);
//Configure JWT Token Authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmployeeDAA.API", Version = "v1" });
    c.MapType<DateTime>(() => new OpenApiSchema { Format = "dd/MMM/yyyy hh:mm tt", Type = "DateTime" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "mypolicy",
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                      });
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.MapControllers();

DataSettingsManager.ApplyUpMigrations(builder.Services.BuildServiceProvider());
app.UseAuthentication();
app.UseRouting();
app.UseCors("mypolicy");
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
