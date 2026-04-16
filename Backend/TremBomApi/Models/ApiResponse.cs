using System.Collections.Generic;

namespace TremBomApi.Models
{
    public class ApiResponse<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
        public List<string>? Erros { get; set; }

        public static ApiResponse<T> Ok(T dados, string mensagem = "Operação realizada com sucesso")
        {
            return new ApiResponse<T>
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        public static ApiResponse<T> Fail(string mensagem, List<string>? erros = null)
        {
            return new ApiResponse<T>
            {
                Sucesso = false,
                Mensagem = mensagem,
                Erros = erros
            };
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioResponse Usuario { get; set; } = new();
    }
}