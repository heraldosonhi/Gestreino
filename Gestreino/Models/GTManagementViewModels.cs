using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Gestreino.Models
{
    public class Users
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [Display(Name = "Utilizador")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [Display(Name = "Nome")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Telefone")]
        public string Phone { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [StringLength(100, ErrorMessage = "A {0} de acesso deve ter o mínimo de {2} caracteres", MinimumLength = 8)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "A {0} de acesso deve conter no mínimo 8 caracteres entre eles maiúsculos e minúsculos, números e caracteres especiais!")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmação da senha")]
        [Compare("Password", ErrorMessage = "A senha de acesso não é idêntica a confirmação.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [Display(Name = "Estado")]
        [DataType(DataType.Text)]
        public string Status { get; set; }

        //[Required]
        [Display(Name = "Data Activação")]
        [DataType(DataType.Text)]
        public string DateAct { get; set; }

        [Display(Name = "Data Desactivação")]
        [DataType(DataType.Text)]
        public string DateDisact { get; set; }

        [Display(Name = "Agendar Activação")]
        public bool ScheduledStatus { get; set; }

        [Display(Name = "Agendado")]
        public string StatusPending { get; set; }
    }
    public class AccessAppendItems
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? ProfileId { get; set; }
        public int? GroupId { get; set; }
        public int? AtomId { get; set; }
    }

}