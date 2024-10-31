using Azure.Core;
using BookStore.CORE.DTOs;
using BookStore.CORE.Models;
using BookStore.CORE.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.EF.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JWT _jwt;

    public AuthRepository(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _jwt = jwt.Value;
        _roleManager = roleManager;
    }

    public async Task<AuthModel> RegisterAsync(RegisterDTO dto)
    {
        ApplicationUser appUser = await _userManager.FindByEmailAsync(dto.Email);
        if (appUser is not null)
            return new AuthModel { Message = "This email already exists." };

        appUser = new ApplicationUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email,
            EmailConfirmed = false
        };

        IdentityResult result = await _userManager.CreateAsync(appUser, dto.Password);

        if (!result.Succeeded)
            return new AuthModel ()
            {
                Message = string.Join(", ", result.Errors.Select(error => error.Description))
            };
        
        var roles = await _userManager.GetRolesAsync(appUser);
        return new AuthModel
        {
            IsAuthenticated = true,
            Username = appUser.UserName,
            Email = appUser.Email,
            Roles = roles.ToList(), 
            Message = "Registration successful."
        };
    }
    public async Task<AuthModel> Login(LoginDTO loginDTO)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

        if (user == null || !_userManager.CheckPasswordAsync(user, loginDTO.Password).Result)
            return new AuthModel()
            {
                Message = "Password Or Email is incorrect",

            };
        var roles = await _userManager.GetRolesAsync(user);
        return new AuthModel()
        {
            Token = GenerateToken(user).Result,
            IsAuthenticated = true,
            Username = user.UserName,
            Email = user.Email,
            Roles = roles.ToList(),
        };
    }
    private async Task<string> GenerateToken(ApplicationUser user)
    {
        List<Claim> claims = new()
             {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        IList<string> roles = await _userManager.GetRolesAsync(user);
        foreach (string role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(_jwt.Key);

        SigningCredentials signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(keyBytes),
            SecurityAlgorithms.HmacSha256
        );

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}