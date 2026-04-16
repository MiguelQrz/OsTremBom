using System.Threading.Tasks;
using TremBomApi.Models;

namespace TremBomApi.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<LoginResponse>> RegistroAsync(RegistroRequest request);
        Task<bool> EmailExisteAsync(string email);
        Task<bool> LogoutAsync(string token);
        Task<UsuarioResponse?> GetUsuarioByTokenAsync(string token);
    }
}