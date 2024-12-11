using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Gestreino.Models
{
    public class Users
    {
        [Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [Display(Name = "Utilizador")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar de Min?")]
        public bool RememberMe { get; set; }
    }

}