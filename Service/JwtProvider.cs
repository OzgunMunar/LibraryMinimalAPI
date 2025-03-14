using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace LibraryMinimalAPI.Service;
public sealed class JwtProvider
{
    public string CreateToken() 
    {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Dummy secret keyDummy secret keyDummy secreDummy secret keyDummy secret keyt keyDummy secret key"));

        JwtSecurityToken jwtSecurityToken= new(
            issuer: "Ozgun Munar",
            audience: "Ozgun Munar",
            claims: null,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512)
        );

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return token;

    }
}