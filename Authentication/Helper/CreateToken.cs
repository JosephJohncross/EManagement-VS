using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EManagementVSA.Authentication.Helper;

public static class GenerateUserToken
{
    public static string CreateToken(IList<Claim> claims, IConfiguration config)
    {
        var jwtSectionSetting = config.GetSection("JwtSettings");
        var securityKey = Encoding.ASCII.GetBytes(jwtSectionSetting.GetValue<string>("Key") ?? string.Empty);

        var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            audience: jwtSectionSetting.GetValue<string>("Audience"),
            issuer: jwtSectionSetting.GetValue<string>("Issuer"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSectionSetting.GetValue<double>("DurationInMinutes"))
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}