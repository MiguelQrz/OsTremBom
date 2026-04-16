using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TremBomApi.Models
{
    public class RegistroRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres")]
        [MaxLength(255)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$", 
            ErrorMessage = "A senha deve conter pelo menos 1 letra maiúscula e 1 número")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarSenha { get; set; } = string.Empty;

        public IFormFile? FotoPerfil { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos uma preferência")]
        [MinLength(1, ErrorMessage = "Selecione pelo menos uma preferência")]
        public List<string> Preferencias { get; set; } = new List<string>();

        [Required(ErrorMessage = "Você deve aceitar os termos de uso")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "Você deve aceitar os termos de uso")]
        public bool TermosAceitos { get; set; }
    }
}