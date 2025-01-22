using System.ComponentModel.DataAnnotations;
using BlazorSecretManager.Entities;
using BlazorSecretManager.Infrastructure;
using eXtensionSharp;
using Microsoft.AspNetCore.Identity;

namespace BlazorSecretManager.Services.Auth;

public interface IAuthService
{
    Task<string> SignIn(string email, string password);
    Task<bool> SignUp(RegisterRequest request);
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    [StringLength(8,  ErrorMessage = "8자 이상 입력해야 합니다.")]
    public string Password { get; set; }
    [Required]
    [StringLength(8,  ErrorMessage = "8자 이상 입력해야 합니다.")]
    public string ConfirmPassword { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// staff...
    /// </summary>
    [Required]
    public string RoleName { get; set; }
    
    public static List<string> Types = new List<string> { "admin", "staff", "guest" };
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> SignIn(string email, string password)
    {
        var user = await _userRepository.GetUser(email);
        var valid = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);
        if (valid == PasswordVerificationResult.Failed) return string.Empty;
        
        var expire = DateTime.UtcNow.AddDays(1);
        return JwtGenerator.GenerateJwtToken(expire, user.Id,  user.Email, user.UserName, "admin");
    }

    public async Task<bool> SignUp(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword) return false;
        var exists = await _userRepository.GetUser(request.Email);
        
        if (exists.xIsNotEmpty()) return false;

        var newItem = new User()
        {
            Email = request.Email,
            UserName = request.Name,
            PhoneNumber = request.PhoneNumber,
            RoleName = request.RoleName,
        };
        
        var hashPassword = _passwordHasher.HashPassword(newItem, request.Password);
        newItem.PasswordHash = hashPassword;

        await this._userRepository.CreateUser(newItem);
        return true;
    }
}