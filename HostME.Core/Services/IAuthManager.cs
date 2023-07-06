using HostME.Core.DTOs;

namespace HostME.Core.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDTO userDTO);

        Task<string> CreateToken();
    }
}
