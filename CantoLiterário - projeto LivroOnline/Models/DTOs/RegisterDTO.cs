using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email Obrigatorio")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Usuario Inválido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Senha Obrigatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string? ConfirmPassword { get; set; }
    }
}
