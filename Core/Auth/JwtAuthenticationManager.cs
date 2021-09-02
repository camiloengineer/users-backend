using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using User.Backend.Api.Core.Exceptions;
using User.Backend.Api.Clases.Domain;

namespace User.Backend.Api.Core.Auth
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly string key;

        public JwtAuthenticationManager( string key)
        {
            this.key = key;
        }

        public async Task<string> AuthenticateAsync(Customer userDB, int DNI, string password, string inDate)
        {
            try
            {
                return await Task.Run(() =>
                {
                    if(userDB == null)
                    {
                        return Task.FromResult("");
                    }
                    if (userDB.DNI != DNI || userDB.Password != Util.Encrypt( password, DNI))
                    {
                        return Task.FromResult("");
                    }


                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenKey = Encoding.ASCII.GetBytes(key);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, userDB.Name),
                            new Claim(ClaimTypes.MobilePhone, userDB.Phone.ToString()),
                            new Claim(ClaimTypes.StreetAddress, userDB.Address),
                            new Claim(ClaimTypes.DateOfBirth, userDB.Birthdate.ToString()),
                            new Claim(ClaimTypes.SerialNumber, userDB.DNI.ToString()),
                        }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    return Task.FromResult(tokenHandler.WriteToken(token));

                });
            }
            catch (Exception ex)
            {
                throw new UserException(string.Format("${0} => {1} $ {2}", ex.Message, ex.StackTrace, inDate));
            }
        }
    }
}
