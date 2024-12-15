using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Gestreino.Models
{
    public class Users
    {
        public int? Id { get; set; }
        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        //[StringLength(100, ErrorMessage = "A {0} de acesso deve ter o mínimo de {2} caracteres", MinimumLength = 32)]
        [Display(Name = "Utilizador")]
        [DataType(DataType.Text)]
        public string Login { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [Display(Name = "Nome")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        //[StringLength(100, ErrorMessage = "A {0} de acesso deve ter o mínimo de {2} caracteres", MinimumLength = 64)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Endereço de email inválido!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
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
        public int Status { get; set; }

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

        [Display(Name = "Gerar senha")]
        public bool isAutomaticPasswordEmail { get; set; }

        [Display(Name = "Enviar senha por email")]
        public bool isAutomaticEmail { get; set; }
    }
    public class AccessAppendItems
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? ProfileId { get; set; }
        public int? GroupId { get; set; }
        public int? AtomId { get; set; }
    }

    public class GRLEndCidade
    {
        public int? ID { get; set; }
        public string NOME { get; set; }
        public string SIGLA { get; set; }
        public int? ENDERECO_PAIS_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PAIS_LIST { get; set; }
    }
    public class GRLEndDistr
    {
        public int? ID { get; set; }
        public string NOME { get; set; }
        public string SIGLA { get; set; }
        public int? ENDERECO_CIDADE_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> CIDADE_LIST { get; set; }
    }



    public class SettingsInst
    {
        public int? ID { get; set; }

        [Display(Name = "Sigla")]
        public string Sigla { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "NIF")]
        public string NIF { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [EmailAddress(ErrorMessage = "{0} não é válido!")]
        [Display(Name = "Email")]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} não é válido!")]
        [StringLength(100, ErrorMessage = "{0} deve ter o mínimo de {2} dígitos", MinimumLength = 9)]
        [Display(Name = "Telefone")]
        public string Telephone { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} não é válido!")]
        [StringLength(100, ErrorMessage = "{0} deve ter o mínimo de {2} dígitos", MinimumLength = 9)]
        [Display(Name = "Telefone Alternativo")]
        public string TelephoneAlternativo { get; set; }

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} não é válido!")]
        [StringLength(100, ErrorMessage = "{0} deve ter o mínimo de {2} dígitos", MinimumLength = 9)]
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Codigo Postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Link URL")]
        public string URL { get; set; }

        [Display(Name = "Número ")]
        public int? Numero { get; set; }

        [Display(Name = "Rua")]
        public string Rua { get; set; }

        [Display(Name = "Morada")]
        public string Morada { get; set; }

        [Display(Name = "País")]
        public int? ENDERECO_PAIS_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PAIS_LIST { get; set; }

        [Display(Name = "Cidade")]
        public int? ENDERECO_CIDADE_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> CIDADE_LIST { get; set; }

        [Display(Name = "Distrito")]
        public int? ENDERECO_MUN_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> MUN_LIST { get; set; }
    }
    public class SettingsDef
    {
        public int? ID { get; set; }

        [Display(Name = "Tema principal")]
        public string INST_PER_TEMA_1 { get; set; }

        [Display(Name = "Cor de texto do sidebar do tema principal")]
        public string INST_PER_TEMA_1_SIDEBAR { get; set; }

        [Display(Name = "Tema secundário")]
        public string INST_PER_TEMA_2 { get; set; }

        [Display(Name = "Tamanho do Logotipo (Pixels)")]
        public int? INST_PER_LOGOTIPO_WIDTH { get; set; }

        [Display(Name = "Moeda padrão")]
        public int? INST_MDL_GPAG_MOEDA_PADRAO { get; set; }

        [Display(Name = "Número de dígitos de valores monetários")]
        public int? INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS { get; set; }

        [Display(Name = "Nota decimal de valores monetários")]
        public int? INST_MDL_GPAG_NOTA_DECIMAL { get; set; }

        [Display(Name = "Número máximo de tentativas de sessão")]
        public int? SEC_SENHA_TENT_BLOQUEIO { get; set; }

        [Display(Name = "Tempo de espera para o desbloqueo de sessão (Minutos)")]
        public int? SEC_SENHA_TENT_BLOQUEIO_TEMPO { get; set; }

        [Display(Name = "Tempo limite de token de email (Minutos)")]
        public int? SEC_SENHA_RECU_LIMITE_EMAIL { get; set; }

        [Display(Name = "Timeout de cookies de sessão (Minutos) ")]
        public int? SEC_SESSAO_TIMEOUT_TEMPO { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> MOEDA_LIST { get; set; }
    }
}