using System;
using System.Collections.Generic;

namespace TremBomApi.Models
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? FotoPerfilUrl { get; set; }
        public List<string> Preferencias { get; set; } = new();
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimoLogin { get; set; }
    }
}