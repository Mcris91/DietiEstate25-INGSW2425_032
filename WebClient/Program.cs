using System.Text.Json;
using DietiEstate.WebClient.ApiService;
using DietiEstate.WebClient.Components;
using DietiEstate.WebClient.Config;
using DietiEstate.WebClient.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton(new JsonSerializerOptions {
    PropertyNameCaseInsensitive = true
});

builder.Services.AddApiService<ListingApiService>(builder.Configuration["ApiUrl"]!, "Listing");
builder.Services.AddApiService<PropertyTypeApiService>(builder.Configuration["ApiUrl"]!, "PropertyType");
builder.Services.AddApiService<OfferApiService>(builder.Configuration["ApiUrl"]!, "Offer");
builder.Services.AddApiService<BookingApiService>(builder.Configuration["ApiUrl"]!, "Booking");
builder.Services.AddApiService<AuthApiService>(builder.Configuration["ApiUrl"]!, "Auth");


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();