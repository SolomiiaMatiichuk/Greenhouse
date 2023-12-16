using System.Net;
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoZoneBooking.BLL.Dtos;
using PhotoZoneBooking.BLL.Exceptions;
using PhotoZoneBooking.BLL.Interfaces;
using PhotoZoneBooking.DAL.Constraints;
using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<AuthCallback> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return new AuthCallback { StatusCode = HttpStatusCode.NotFound };
        }
        
        var authResult = await _userManager.CheckPasswordAsync(user, password);
        if (!authResult)
        {
            return new AuthCallback { StatusCode = HttpStatusCode.Unauthorized };
        }

        var userRole = await _userManager.GetRolesAsync(user);

        var identity = new ClaimsIdentity(new[]
        {
            new Claim("id", user.Id),
            new Claim(ClaimTypes.Role, userRole[0]),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "user")
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        return new AuthCallback
        {
            StatusCode = HttpStatusCode.OK,
            ClaimsIdentity = identity
        };
    }

    public async Task<AuthCallback> RegisterAsync(RegisterDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is not null)
        {
            return new AuthCallback { StatusCode = HttpStatusCode.Conflict };
        }

        var newUser = new User
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var identityResult = await _userManager.CreateAsync(newUser, model.Password);
        
        if (!identityResult.Succeeded)
        {
            return new AuthCallback { StatusCode = HttpStatusCode.InternalServerError };
        }

        var roleResult = await _userManager.AddToRoleAsync(newUser, SystemRoleConstraints.UserRole);
        
        if (!roleResult.Succeeded)
        {
            return new AuthCallback { StatusCode = HttpStatusCode.InternalServerError };
        }

        return new AuthCallback
        {
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<string> GenerateResetPasswordTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return String.Empty;
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return String.Empty;
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task ChangePassword(string newPassword, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null) return;
        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        var a = 2;
    }

    public async Task ConfirmEmailAsync(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var result = await _userManager.ConfirmEmailAsync(user, token);
    }

    public async Task EditUserAsync(UserEditDto model, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        user.LastName = model.LastName;
        user.FirstName = model.FirstName;
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.DeleteAsync(user);
    }
}