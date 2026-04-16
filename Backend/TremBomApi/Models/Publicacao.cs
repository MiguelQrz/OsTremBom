using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TremBomApi.Models
{
    [Table("publicacoes")]
    public class Publicacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("local_id")]
        public int LocalId { get; set; }

        [Column("local_nome")]
        [MaxLength(255)]
        public string? LocalNome { get; set; }

        [Column("feedback")]
        public string? Feedback { get; set; }

        [Range(1, 5)]
        [Column("rating")]
        public int Rating { get; set; }

        [Column("total_likes")]
        public int TotalLikes { get; set; }

        [Column("total_comentarios")]
        public int TotalComentarios { get; set; }

        [Column("total_compartilhamentos")]
        public int TotalCompartilhamentos { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("data_atualizacao")]
        public DateTime? DataAtualizacao { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        // Relacionamentos
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("LocalId")]
        public virtual Local? Local { get; set; }

        public virtual ICollection<PublicacaoFoto> Fotos { get; set; } = new List<PublicacaoFoto>();
        public virtual ICollection<PublicacaoLike> Likes { get; set; } = new List<PublicacaoLike>();
        public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }

    [Table("publicacoes_fotos")]
    public class PublicacaoFoto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("publicacao_id")]
        public int PublicacaoId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("foto_url")]
        public string FotoUrl { get; set; } = string.Empty;

        [Column("ordem")]
        public int Ordem { get; set; }

        [ForeignKey("PublicacaoId")]
        public virtual Publicacao? Publicacao { get; set; }
    }

    [Table("publicacoes_likes")]
    public class PublicacaoLike
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("publicacao_id")]
        public int PublicacaoId { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("data_like")]
        public DateTime DataLike { get; set; } = DateTime.Now;

        [ForeignKey("PublicacaoId")]
        public virtual Publicacao? Publicacao { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }

    [Table("comentarios")]
    public class Comentario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("publicacao_id")]
        public int PublicacaoId { get; set; }

        [Required]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Required]
        [Column("texto")]
        public string Texto { get; set; } = string.Empty;

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("data_atualizacao")]
        public DateTime? DataAtualizacao { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        [ForeignKey("PublicacaoId")]
        public virtual Publicacao? Publicacao { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }
    }
}