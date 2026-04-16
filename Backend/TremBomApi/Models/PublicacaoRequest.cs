using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TremBomApi.Models
{
    public class PublicacaoRequest
    {
        [Required(ErrorMessage = "O local é obrigatório")]
        public int LocalId { get; set; }

        [MaxLength(255)]
        public string? LocalNome { get; set; }

        [Required(ErrorMessage = "O feedback é obrigatório")]
        [MinLength(3, ErrorMessage = "O feedback deve ter pelo menos 3 caracteres")]
        public string Feedback { get; set; } = string.Empty;

        [Required(ErrorMessage = "A avaliação é obrigatória")]
        [Range(1, 5, ErrorMessage = "A avaliação deve ser entre 1 e 5 estrelas")]
        public int Rating { get; set; }

        public List<IFormFile>? Fotos { get; set; }
    }

    public class ComentarioRequest
    {
        [Required(ErrorMessage = "O texto do comentário é obrigatório")]
        [MinLength(1, ErrorMessage = "O comentário não pode estar vazio")]
        [MaxLength(500, ErrorMessage = "O comentário deve ter no máximo 500 caracteres")]
        public string Texto { get; set; } = string.Empty;
    }

    public class PublicacaoResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioUsername { get; set; } = string.Empty;
        public string? UsuarioAvatar { get; set; }
        public int LocalId { get; set; }
        public string LocalNome { get; set; } = string.Empty;
        public string? Feedback { get; set; }
        public int Rating { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComentarios { get; set; }
        public int TotalCompartilhamentos { get; set; }
        public bool UsuarioCurtiu { get; set; }
        public List<string> Fotos { get; set; } = new();
        public List<ComentarioResponse> Comentarios { get; set; } = new();
        public DateTime DataCriacao { get; set; }
        public string DataFormatada { get; set; } = string.Empty;
    }

    public class ComentarioResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioUsername { get; set; } = string.Empty;
        public string? UsuarioAvatar { get; set; }
        public string Texto { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public string DataFormatada { get; set; } = string.Empty;
    }

    public class LocalResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string? Descricao { get; set; }
        public string? ImagemUrl { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComentarios { get; set; }
        public string LikesFormatado { get; set; } = string.Empty;
        public string ComentariosFormatado { get; set; } = string.Empty;
    }
}