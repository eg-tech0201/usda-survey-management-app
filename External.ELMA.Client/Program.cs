using External.ELMA.Client.Authentication;
using External.ELMA.Client.Configuration;
using External.ELMA.Client.Endpoints;
using External.ELMA.Client.Services;
using app_services.Contracts.Integration;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ElmaClientOptions>(builder.Configuration.GetSection(ElmaClientOptions.SectionName));
builder.Services.AddAuthentication(ElmaAuthenticationDefaults.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, ElmaClientAuthenticationHandler>(ElmaAuthenticationDefaults.SchemeName, _ => { });
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IElmaDownstreamClient, StubElmaDownstreamClient>();
builder.Services.AddSingleton<IElmaCircuitBreaker, InMemoryElmaCircuitBreaker>();
builder.Services.AddSingleton<IElmaGateway, ResilientElmaGateway>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapElmaClientEndpoints();

app.Run();
