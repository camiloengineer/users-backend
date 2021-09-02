
using System.Threading.Tasks;
using User.Backend.Api.Clases.Domain;

namespace User.Backend.Api.Core.Auth
{
    public interface IJwtAuthenticationManager
    {
        Task<string> AuthenticateAsync(Customer userDB, int DNI, string password, string inDate);
    }
}
