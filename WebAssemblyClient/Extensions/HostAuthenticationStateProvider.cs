using System.IdentityModel.Tokens.Jwt;

namespace WebAssemblyClient.Extensions;

using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

public class HostAuthenticationStateProvider(IJSRuntime js) : AuthenticationStateProvider
{
    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await js.InvokeAsync<string>("getCookies", "id_token");
            
            if (string.IsNullOrEmpty(token)) return _anonymous;

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "JwtAuth");
            var principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }
        catch
        {
            return _anonymous;
        }
    }

    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var claims = token.Claims.ToList();
        
        var id = claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        if (id != null)
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
        
        var role = claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (role != null) 
            claims.Add(new Claim(ClaimTypes.Role, role));
        
        var email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
        if (email != null) 
            claims.Add(new Claim(ClaimTypes.Name, email));
        
        var firstName = claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
        if (firstName != null)
            claims.Add(new Claim(ClaimTypes.GivenName, firstName));

        var lastName = claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
        if (lastName != null)
            claims.Add(new Claim(ClaimTypes.Surname, lastName));

        return claims;
    }
}