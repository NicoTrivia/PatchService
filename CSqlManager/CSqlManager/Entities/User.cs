using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using System.Text.Json;
namespace CSqlManager;

public class User
{
    public int id { get; set; }
    public string login { get; set; }
    public string? lastname { get; set; }
    public string? firstname { get; set; }
    public string? email { get; set; }
    public string? password { get; set; } // encrypted 
    public string tenant  { get; set; }
    public bool active  { get; set; }
    public string? profile { get; set; }

    public string? jwt  { get; set; }
    private static readonly string jwtKey = Variables.RetrieveVariable("jwtKey").ToString() ?? throw new InvalidOperationException();
    public static string GenerateJwtToken(string userLogin, int userId, string tenant, string profile, string? email)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(jwtKey);
        byte[] hashedKeyBytes;

        using (var sha256 = SHA256.Create())
        {
            hashedKeyBytes = sha256.ComputeHash(keyBytes);
        }

        var claims = new[]
        {
            new Claim("APP", "Patch Services"),
            new Claim("User", userLogin),
            new Claim("UserId", userId.ToString()),
            new Claim("UserEmail", email == null ? "" : email),
            new Claim("Tenant", tenant),
            new Claim("Profile", profile)
        };

        var key = new SymmetricSecurityKey(hashedKeyBytes);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours((profile == "ADMIN" || profile == "OPERATOR") ? 10 : 2), // Set the token expiration time
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}