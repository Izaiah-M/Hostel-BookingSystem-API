using HostME.Core.DTOs;
using HostME.Data.Models;

namespace HostME.Core.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDTO userDTO);

        Task<string> CreateToken(ApiUser user);
    }
}
