using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using WebAssemblyClient.Extensions;
using WebAssemblyClient;
using WebAssemblyClient.ApiService;
using WebAssemblyClient.Config;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7228") });

builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton(new JsonSerializerOptions {
    PropertyNameCaseInsensitive = true
});
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddScoped<HostAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<HostAuthenticationStateProvider>());

builder.Services.AddApiService<ListingApiService>(builder.Configuration["ApiUrl"]!, "Listing");
builder.Services.AddApiService<PropertyTypeApiService>(builder.Configuration["ApiUrl"]!, "PropertyType");
builder.Services.AddApiService<OfferApiService>(builder.Configuration["ApiUrl"]!, "Offer");
builder.Services.AddApiService<BookingApiService>(builder.Configuration["ApiUrl"]!, "Booking");
builder.Services.AddApiService<AuthApiService>(builder.Configuration["ApiUrl"]!, "Auth");


builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

await builder.Build().RunAsync();
