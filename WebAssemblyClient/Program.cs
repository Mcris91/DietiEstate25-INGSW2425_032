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

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(
    _ => new HttpClient {BaseAddress = new Uri(apiBaseUrl)}
    );

builder.Services.AddFluentUIComponents();

builder.Services.AddSingleton(new JsonSerializerOptions {
    PropertyNameCaseInsensitive = true
});
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddScoped<HostAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<HostAuthenticationStateProvider>());

builder.Services.AddApiService<ListingApiService>(apiBaseUrl, "Listing");
builder.Services.AddApiService<PropertyTypeApiService>(apiBaseUrl, "PropertyType");
builder.Services.AddApiService<FavouritesApiService>(apiBaseUrl, "Favourites");
builder.Services.AddApiService<OfferApiService>(apiBaseUrl, "Offer");
builder.Services.AddApiService<BookingApiService>(apiBaseUrl, "Booking");
builder.Services.AddApiService<AuthApiService>(apiBaseUrl, "Auth");
builder.Services.AddApiService<UserApiService>(apiBaseUrl, "User");

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

await builder.Build().RunAsync();
