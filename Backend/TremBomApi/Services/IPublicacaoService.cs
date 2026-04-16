using System.Collections.Generic;
using System.Threading.Tasks;
using TremBomApi.Models;

namespace TremBomApi.Services
{
    public interface IPublicacaoService
    {
        // Publicações
        Task<ApiResponse<PublicacaoResponse>> CriarPublicacaoAsync(int usuarioId, PublicacaoRequest request);
        Task<ApiResponse<List<PublicacaoResponse>>> GetPublicacoesAsync(int? usuarioId = null);
        Task<ApiResponse<PublicacaoResponse>> GetPublicacaoByIdAsync(int id, int? usuarioAtualId = null);
        Task<ApiResponse<bool>> CurtirPublicacaoAsync(int publicacaoId, int usuarioId);
        Task<ApiResponse<bool>> DescurtirPublicacaoAsync(int publicacaoId, int usuarioId);
        Task<ApiResponse<bool>> ExcluirPublicacaoAsync(int publicacaoId, int usuarioId);

        // Comentários
        Task<ApiResponse<ComentarioResponse>> AdicionarComentarioAsync(int publicacaoId, int usuarioId, ComentarioRequest request);
        Task<ApiResponse<List<ComentarioResponse>>> GetComentariosAsync(int publicacaoId);
        Task<ApiResponse<bool>> ExcluirComentarioAsync(int comentarioId, int usuarioId);

        // Locais
        Task<ApiResponse<List<LocalResponse>>> GetLocaisAsync();
        Task<ApiResponse<LocalResponse>> GetLocalByIdAsync(int id);
        Task<ApiResponse<LocalResponse>> CriarLocalAsync(Local local);
    }
}