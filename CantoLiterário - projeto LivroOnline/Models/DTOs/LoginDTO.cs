using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Senha Inválida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Usuário Inválido")]
        public string UserName { get; set; }
        [DisplayName("Lembrar de Mim")]
        public bool RememberMe { get; set; }
    }
}
