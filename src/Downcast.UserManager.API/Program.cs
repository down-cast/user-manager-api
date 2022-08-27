using Downcast.Common.Errors.Handler.Config;
using Downcast.Common.Logging;
using Downcast.SessionManager.SDK.Authentication.Handler;
using Downcast.UserManager.API.Config;

using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("http-clients-settings.json");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDowncastAuthentication(builder.Configuration);

builder.AddUserManagerServices();
builder.AddSerilog();
builder.AddErrorHandlerOptions();

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();
app.UseErrorHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseForwardedHeaders();

app.MapControllers();

app.Run();