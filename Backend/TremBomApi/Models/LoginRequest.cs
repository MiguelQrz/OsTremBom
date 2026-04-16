using System.ComponentModel.DataAnnotations;

namespace TremBomApi.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        public string Senha { get; set; } = string.Empty;

        public bool Lembrar { get; set; }
    }
}