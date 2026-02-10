using DietiEstate.Infrastracture;
using DietiEstate.Infrastructure;
using Hangfire;
using Microsoft.Extensions.Hosting;
using DotNetEnv;
using Hangfire.PostgreSql;

var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(options => { 
        options.UseNpgsqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
    });
});

builder.Services.AddHangfireServer();

var app = builder.Build();
await app.RunAsync();
