using Downcast.Common.Errors.Handler.Config;
using Downcast.Common.Logging;
using Downcast.SessionManager.SDK.Authentication.Handler;
using Downcast.UserManager.API.Config;
using Downcast.UserManager.Model.Validators;
using Downcast.UserManager.PasswordManager;
using Downcast.UserManager.Repository.Config;

using Microsoft.OpenApi.Models;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("http-clients-settings.json");
builder.Configuration.AddJsonFile("password-requirements.json");
builder.Configuration.AddJsonFile("hashing-settings.json");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
       }
    });
});
builder.Services.AddDowncastAuthentication(builder.Configuration);
builder.Services.AddOptions<PasswordRequirementOptions>()
    .Bind(builder.Configuration.GetSection(PasswordRequirementOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.Configure<HashingOptions>(builder.Configuration.GetSection(HashingOptions.SectionName));
builder.AddUserManagerServices();
builder.AddFirestoreRepositoryConfigurations();
builder.AddSerilog();
builder.AddErrorHandlerOptions();

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();
app.UseErrorHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseHsts();

app.UseAuthentication();
app.UseAuthorization();
app.UseForwardedHeaders();

app.MapControllers();

app.Run();