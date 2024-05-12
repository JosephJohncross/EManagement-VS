using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EManagementVSA.Entities;
using Microsoft.IdentityModel.Tokens;

namespace EManagementVSA.Authentication.Helper;

public static class GenerateUserToken
{
    public static string CreateToken(IConfiguration config, List<Claim> claims)
    {
        var jwtSectionSetting = config.GetSection("JwtSettings");
        var securityKey = Encoding.ASCII.GetBytes(jwtSectionSetting.GetValue<string>("Key") ?? string.Empty);

        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            audience: jwtSectionSetting.GetValue<string>("Audience"),
            claims: claims,
            issuer: jwtSectionSetting.GetValue<string>("Issuer"),
            expires: DateTime.UtcNow.AddMinutes(jwtSectionSetting.GetValue<double>("DurationInMinutes")),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
