using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazorSecretManager.Services;
using eXtensionSharp;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SecretManager;

namespace BlazorSecretManager.Infrastructure;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _protectedSessionStore;
    private readonly IUserRepository _userRepository;
    private CircuitHandler _circuitHandler;
    public CustomAuthStateProvider(ProtectedSessionStorage protectedSessionStore,
        IUserRepository userRepository,
        CircuitHandler circuitHandler)
    {
        _protectedSessionStore = protectedSessionStore;
        _userRepository = userRepository;
        _circuitHandler = circuitHandler;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_circuitHandler.xAs<CustomCircuitHandler>().IsPrerendering.xIsFalse())
        {
            try
            {
                var tokenString = await _protectedSessionStore.GetAsync<string>(Constants.JwtCacheKey);

                if (IsTokenValid(tokenString.Value) == false)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var jwtToken = new JwtSecurityToken(tokenString.Value);

                var identity = new ClaimsIdentity(jwtToken.Claims, Constants.JwtCacheKey);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                var id = claimsPrincipal.Claims.First(m => m.Type == ClaimTypes.NameIdentifier)?.Value;
                if (id.xIsNotEmpty())
                {
                    var user = await _userRepository.GetUser(id);
                    if (user.xIsNotEmpty())
                    {
                        await _protectedSessionStore.DeleteAsync(Constants.JwtCacheKey);
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));                        
                    }
                }
                return new AuthenticationState(claimsPrincipal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
    

    public event Action OnChange;
    public void NotifyUserAuthentication(string tokenString)
    {
        var jwtToken = new JwtSecurityToken(tokenString);
        var identity = new ClaimsIdentity(jwtToken.Claims, Constants.JwtCacheKey);
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
        OnChange?.Invoke();
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
        OnChange?.Invoke();
    }

    private bool IsTokenValid(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token))
            return false;

        var jwtToken = tokenHandler.ReadJwtToken(token);
        if (jwtToken.xIsNotEmpty())
        {
            if (jwtToken.Payload.Expiration.xIsNotEmpty())
            {
                var expiration = jwtToken.Payload.Expiration;
                var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expiration.GetValueOrDefault()).DateTime;
                return expirationDate > DateTime.UtcNow;                
            }
        }
        
        return false;
    }
}

public class CustomCircuitHandler : CircuitHandler
{
    public bool IsPrerendering { get; private set; } = true;

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // 클라이언트 연결이 이루어진 시점
        IsPrerendering = false;
        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        // 클라이언트 연결이 종료된 시점
        return Task.CompletedTask;
    }
}