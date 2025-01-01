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

        [Display(Name = "Código Postal")]
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

        [Display(Name = "Município")]
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


    public class Athlete
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        public int? Age { get; set; }

        [Display(Name = "n° Sócio")]
        public int? Numero { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Sexo")]
        public int? Sexo { get; set; }

        [Display(Name = "Data nascimento")]
        public string DataNascimento { get; set; }

        [Display(Name = "Estado Civil")]
        public int? EstadoCivil { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> EstadoCivilList { get; set; }

        [Display(Name = "NIF")]
        public string NIF { get; set; }

        [Display(Name = "Nacionalidade")]
        public int[] NacionalidadeId { get; set; }

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

        [Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Link URL")]
        public string URL { get; set; }
        [Display(Name = "Número ")]
        public int? EndNumero { get; set; }

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

        [Display(Name = "Município")]
        public int? ENDERECO_MUN_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> MUN_LIST { get; set; }

        [Display(Name = "Tipo")]
        public int? ENDERECO_TIPO { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ENDERECO_TIPO_LIST { get; set; }

        [Display(Name = "Natural de")]
        public int? NAT_PAIS_ID { get; set; }

        [Display(Name = "Cidade")]
        public int? NAT_CIDADE_ID { get; set; }

        [Display(Name = "Município")]
        public int? NAT_MUN_ID { get; set; }


        [Display(Name = "Altura (cm)")]
        public string Caract_Altura { get; set; }

        [Display(Name = "VO2")]
        public int? Caract_VO2 { get; set; }

        [Display(Name = "Peso (kg)")]
        public string Caract_Peso { get; set; }

        [Display(Name = "Massa Gorda (%)")]
        public int? Caract_MassaGorda { get; set; }

        [Display(Name = "IMC")]
        public int? Caract_IMC { get; set; }

        [Display(Name = "Duração do plano")]
        public int? Caract_DuracaoPlano { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Caract_DuracaoPlanoList { get; set; }

        [Display(Name = "FC Repouso")]
        public int? Caract_FCRepouso { get; set; }

        [Display(Name = "Protocolo")]
        public int? Caract_Protocolo { get; set; }

        [Display(Name = "FC Máximo")]
        public int? Caract_FCMaximo { get; set; }

        [Display(Name = "TA Sistólica")]
        public int? Caract_TASistolica { get; set; }

        [Display(Name = "TA Distólica")]
        public int? Caract_TADistolica { get; set; }
        //
        [Display(Name = "Hipertensão")]
        public bool FR_Hipertensao { get; set; }

        [Display(Name = "Tabaco")]
        public bool FR_Tabaco { get; set; }

        [Display(Name = "Hiperlipidemia")]
        public bool FR_Hiperlipidemia { get; set; }

        [Display(Name = "Obesidade")]
        public bool FR_Obesidade { get; set; }

        [Display(Name = "Diabetes")]
        public bool FR_Diabetes { get; set; }

        [Display(Name = "Inactividade")]
        public bool FR_Inactividade { get; set; }

        [Display(Name = "Heriditariedade")]
        public bool FR_Heriditariedade { get; set; }

        [Display(Name = "Exames complementares")]
        public bool FR_Examescomplementares{ get; set; }

        [Display(Name = "Outros")]
        public bool FR_Outros { get; set; }
        //
        [Display(Name = "Actividade")]
        public bool OB_Actividade { get; set; }

        [Display(Name = "Controlo de peso")]
        public bool OB_Controlopeso { get; set; }

        [Display(Name = "Predevenir a \"Idade\"")]
        public bool OB_PrevenirIdade{ get; set; }

        [Display(Name = "Treino desportivo")]
        public bool OB_TreinoDesporto{ get; set; }

        [Display(Name = "Aumentar a massa muscular")]
        public bool OB_AumentarMassa { get; set; }

        [Display(Name = "Bem estar / Saúde")]
        public bool OB_BemEstar { get; set; }

        [Display(Name = "Tonificar")]
        public bool OB_Tonificar { get; set; }

        [Display(Name = "Outros")]
        public bool OB_Outros { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_TIPO_IDENTIFICACAO_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_FAMILIARES_GRUPOS_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSAO_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_Contracto_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_Regime_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_DEFICIENCIA_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_DEFICIENCIA_GRAU_LIST { get; set; }

    }



    public class PES_Dados_Pessoais_Professional
    {
        public int? ID { get; set; }
        [Display(Name = "Regime")]
        public int? PES_PROFISSOES_REGIME_ID { get; set; }

        [Display(Name = "Contrato")]
        public int? PES_PROFISSOES_CONTRACTO_ID { get; set; }

        [Display(Name = "Profissao")]
        public int? PES_PROFISSAO_ID { get; set; }
        
        [Display(Name = "Data Inicio")]
        public string DateIni { get; set; }

        [Display(Name = "Data Fim")]
        public string DateEnd { get; set; }

        [Display(Name = "Descricao")]
        public string Descricao { get; set; }

        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSAO_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSOES_CONTRACTO_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSOES_REGIME_LIST { get; set; }
    }
    public class PES_Dados_Pessoais_Agregado
    {
        public int? ID { get; set; }
        public int? PES_PESSOAS_ID { get; set; }
        [Display(Name = "Agregado")]
        public int? PES_FAMILIARES_GRUPOS_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_FAMILIARES_GRUPOS_LIST { get; set; }

        [Display(Name = "Profissao")]
        public int? PES_PROFISSAO_ID { get; set; }
        [Display(Name = "Nome")]
        public string Nome { get; set; }

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

        //[Required(ErrorMessage = "{0} é um campo obrigatório!")]
        //[StringLength(100, ErrorMessage = "A {0} de acesso deve ter o mínimo de {2} caracteres", MinimumLength = 64)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Endereço de email inválido!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "URL")]
        public string URL { get; set; }

        [Display(Name = "Número ")]
        public int? Numero { get; set; }

        [Display(Name = "Rua")]
        public string Rua { get; set; }

        [Display(Name = "Morada")]
        public string Morada { get; set; }

        [Display(Name = "País")]
        public int? PaisId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PaisList { get; set; }

        [Display(Name = "Cidade")]
        public int? CidadeId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> CidadeList { get; set; }

        [Display(Name = "Município")]
        public int? DistrictoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> DistrictoList { get; set; }

        [Display(Name = "Isento")]
        public bool Isento { get; set; }

        [Display(Name = "Data Fim")]
        public string DateEnd { get; set; }

        [Display(Name = "Descricao")]
        public string Descricao { get; set; }

        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSAO_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSOES_CONTRACTO_LIST { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_PROFISSOES_REGIME_LIST { get; set; }
    }
    public class PES_Dados_Pessoais_Deficiencia
    {
        public int? ID { get; set; }
        public int? PES_PESSOAS_ID { get; set; }

        [Display(Name = "Deficiencia")]
        public int? PES_DEFICIENCIA_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_DEFICIENCIA_LIST { get; set; }

        [Display(Name = "Grau de deficiencia")]
        public int? PES_DEFICIENCIA_GRAU_ID { get; set; }

        [Display(Name = "Descricao")]
        public string Descricao { get; set; }

    }
    public class GTAvaliado
    {
        public int? ID { get; set; }

        [Display(Name = "Avaliado")]
        public int? AthleteId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> AthleteList { get; set; }
    }
    public class PES_Dados_Pessoais_Ident
    {
        public int? ID { get; set; }
        public int? PES_PESSOAS_ID { get; set; }

        [Display(Name = "Tipo de Identificação")]
        public int? PES_TIPO_IDENTIFICACAO { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_TIPO_IDENTIFICACAO_LIST { get; set; }
        [Display(Name = "Número")]
        public string Numero { get; set; }

        //[Required]
        [Display(Name = "Data de Emissão")]
        [DataType(DataType.Text)]
        public string DateIssue { get; set; }

        [Display(Name = "Data de Caducidade")]
        [DataType(DataType.Text)]
        public string DateExpire { get; set; }

        [Display(Name = "Órgão Emissor")]
        public string OrgaoEmissor { get; set; }

        [Display(Name = "País")]
        public int? PaisId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PaisList { get; set; }

        [Display(Name = "Cidade")]
        public int? CidadeId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> CidadeList { get; set; }

        [Display(Name = "Observação")]
        [DataType(DataType.Text)]
        public string Observacao { get; set; }
    }
    public class GT_FaseTreino
    {
        public int? ID { get; set; }

        [Display(Name = "Fase do treino")]
        public string SIGLA { get; set; }

        [Display(Name = "Séries")]
        public int? GT_Series_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Series_List { get; set; }

        [Display(Name = "Repetições")]
        public int? GT_Repeticoes_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Repeticoes_List { get; set; }

        [Display(Name = "Perc. RM")]
        public int? GT_Carga_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Carga_List { get; set; }

        [Display(Name = "Descanso")]
        public int? GT_TempoDescanso_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TempoDescanso_List { get; set; }
    }
    public class GTExercicio
    {
        public int? ID { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Treino")]
        public int? TipoTreinoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TipoList { get; set; }

        [Display(Name = "Alongamento")]
        public int? Alongamento { get; set; }
        [Display(Name = "Sequência")]
        public int? Sequencia { get; set; }
        
    }
    public class GT_TreinoBodyMass
    {
        public int? ID { get; set; }
        public bool? predefined { get; set; }
        public int? GTTipoTreinoId { get; set; }
        public int? PEsId { get; set; }
        [Display(Name = "Periodização:")]
        public int? Periodizacao { get; set; }

        [Display(Name = "N° de séries:")]
        public int? GT_Series_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Series_List { get; set; }

        [Display(Name = "N° de repetições:")]
        public int? GT_Repeticoes_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Repeticoes_List { get; set; }

        [Display(Name = "% 1RM:")]
        public int? GT_Carga_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Carga_List { get; set; }

        [Display(Name = "Tempo descanso:")]
        public int? GT_TempoDescanso_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TempoDescanso_List { get; set; }
        [Display(Name = "Fases de treino:")]
        public int? FaseTreinoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FaseTreinoList { get; set; }
        [Display(Name = "Nome do treino:")]
        public int? GTTreinoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem>GTTreinoList { get; set; }

        [Display(Name = "Duração (Min.):")]
        public int? GT_DuracaoTreinoCardio_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_DuracaoTreinoCardioList { get; set; }

        public List<ExerciseArq> ExerciseArqList { get; set; }
        public List<ExerciseArq> ExerciseArqListTreino { get; set; }

        //[Required]
        [Display(Name = "Data de início:")]
        [DataType(DataType.Text)]
        public string DateIni { get; set; }

        [Display(Name = "Data de fim:")]
        [DataType(DataType.Text)]
        public string DateEnd { get; set; }
        [Display(Name = "RM")]
        public string RM { get; set; }

        [Display(Name = "Carga")]
        public int? CargaUsada { get; set; }

        [Display(Name = "Rep's:")]
        public int? Reps { get; set; }
        [Display(Name = "Nome:")]
        public string Nome { get; set; }

        [Display(Name = "Observações:")]
        public string Observacoes { get; set; }

        [Display(Name = "FC (Min/Máx) bpm:")]
        public int? FC { get; set; }

        [Display(Name = "Nível / Resist. / Velocidade:")]
        public int? Nivel { get; set; }

        [Display(Name = "Inclinação:")]
        public string Distancia { get; set; }
    }

    public class ExerciseArq
    {
        public int? ExerciseId { get; set; }
        public string Name { get; set; }
        public string LogoPath { get; set; }

        public int? GT_Treino_ID { get; set; }
        public int? GT_Series_ID { get; set; }
        public int? GT_Repeticoes_ID { get; set; }
        public int? GT_TempoDescanso_ID { get; set; }
        public int? GT_Carga_ID { get; set; }
        public int? REPETICOES_COMPLETADAS { get; set; }
        public int? CARGA_USADA { get; set; }
        public decimal? ONERM { get; set; }
        public int? GT_DuracaoTreinoCardio_ID { get; set; }
        public int? FC { get; set; }
        public int? Nivel { get; set; }
        public decimal? Distancia { get; set; }
        public int? ORDEM { get; set; }
    }

    public class GT_Quest_Anxient
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        public int? q1 { get; set; }
        public int? q2 { get; set; }
        public int? q3 { get; set; }
        public int? q4 { get; set; }
        public int? q5 { get; set; }
        public int? q6 { get; set; }
        public int? q7 { get; set; }
        public int? q8 { get; set; }
        public int? q9 { get; set; }
        public int? q10 { get; set; }
        public int? q11 { get; set; }
        public int? q12{ get; set; }
        public int? q13 { get; set; }
        public int? q14 { get; set; }
        public int? Summary { get; set; }
        public string SummaryDesc { get; set; }
    }

}