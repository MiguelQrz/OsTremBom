using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TremBomApi.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres")]
        [MaxLength(255)]
        [Column("nome_completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        [Column("telefone")]
        public string? Telefone { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("senha_hash")]
        public string SenhaHash { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("foto_perfil_url")]
        public string? FotoPerfilUrl { get; set; }

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Column("termos_aceitos_em")]
        public DateTime? TermosAceitosEm { get; set; }

        [Column("ultimo_login")]
        public DateTime? UltimoLogin { get; set; }

        // Relacionamentos
        public virtual ICollection<UsuarioPreferencia> Preferencias { get; set; } = new List<UsuarioPreferencia>();
        public virtual ICollection<Sessao> Sessoes { get; set; } = new List<Sessao>();
    }

    [Table("usuarios_preferencias")]
    public class UsuarioPreferencia
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("preferencia")]
        public string Preferencia { get; set; } = string.Empty;

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }

    [Table("sessoes")]
    public class Sessao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("token")]
        public string Token { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("token_lembrar")]
        public string? TokenLembrar { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("data_expiracao")]
        public DateTime? DataExpiracao { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }

    public static class PreferenciasDisponiveis
    {
        public static readonly string[] Valores = new[]
        {
            "barzinho", "bar", "samba", "rock", "jazz",
            "cafe", "adega", "teatro", "cinema"
        };

        public static readonly Dictionary<string, string> Descricoes = new()
        {
            { "barzinho", "🍻 Barzinho" },
            { "bar", "🥃 Bar" },
            { "samba", "🥁 Samba" },
            { "rock", "🎸 Rock" },
            { "jazz", "🎷 Jazz" },
            { "cafe", "☕ Café" },
            { "adega", "🍷 Adega" },
            { "teatro", "🎭 Teatro" },
            { "cinema", "🎬 Cinema" }
        };
    }
}