using DocumentFormat.OpenXml.Spreadsheet;
using System;
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
        public int? PesId { get; set; }
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
        [Display(Name = "Telefone A/")]
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

        [Display(Name = "Timeout de cookies de sessão (Minutos)")]
        public int? SEC_SESSAO_TIMEOUT_TEMPO { get; set; }

        [Display(Name = "Host ou Endereço IP SMTP")]
        public string NET_STMP_HOST { get; set; }

        [Display(Name = "Porta SMTP")]
        public int? NET_STMP_PORT { get; set; }

        [Display(Name = "Utilizador SMTP")]
        public string NET_SMTP_USERNAME { get; set; }

        [Display(Name = "Senha do utilizador SMTP")]
        public string NET_SMTP_SENHA { get; set; }

        [Display(Name = "Endereço de resposta")]
        public string NET_SMTP_FROM { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> MOEDA_LIST { get; set; }
    }


    public class Athlete
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        public int? Age { get; set; }

        [Display(Name = "N° Sócio")]
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

        [Display(Name = "VO2 (ml/kg/min)")]
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
        public decimal? Caract_FCMaximo { get; set; }

        [Display(Name = "TA Sistólica")]
        public decimal? Caract_TASistolica { get; set; }

        [Display(Name = "TA Distólica")]
        public decimal? Caract_TADistolica { get; set; }
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
        public bool FR_Examescomplementares { get; set; }

        [Display(Name = "Outros")]
        public bool FR_Outros { get; set; }
        //
        [Display(Name = "Actividade")]
        public bool OB_Actividade { get; set; }

        [Display(Name = "Controlo de peso")]
        public bool OB_Controlopeso { get; set; }

        [Display(Name = "Predevenir a \"Idade\"")]
        public bool OB_PrevenirIdade { get; set; }

        [Display(Name = "Treino desportivo")]
        public bool OB_TreinoDesporto { get; set; }

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

        public decimal? FCTreino1 { get; set; }
        public decimal? FCTreino2 { get; set; }
        public decimal? FCTreino3 { get; set; }
        public decimal? FCTreino4 { get; set; }
        public decimal? FCTreino5 { get; set; }
        public decimal? FCTreino6 { get; set; }
        public decimal? FCTreino7 { get; set; }
        public decimal? FCTreino8 { get; set; }
        public decimal? FCTreino9 { get; set; }
        public decimal? FCTreino10 { get; set; }

    }



    public class PES_Dados_Pessoais_Professional
    {
        public int? ID { get; set; }
        [Display(Name = "Regime")]
        public int? PES_PROFISSOES_REGIME_ID { get; set; }

        [Display(Name = "Contrato")]
        public int? PES_PROFISSOES_CONTRACTO_ID { get; set; }

        [Display(Name = "Profissão")]
        public int? PES_PROFISSAO_ID { get; set; }

        [Display(Name = "Data Início")]
        public string DateIni { get; set; }

        [Display(Name = "Data Fim")]
        public string DateEnd { get; set; }

        [Display(Name = "Descrição")]
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

        [Display(Name = "Profissão")]
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

        [Display(Name = "Deficiência")]
        public int? PES_DEFICIENCIA_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> PES_DEFICIENCIA_LIST { get; set; }

        [Display(Name = "Grau de deficiência")]
        public int? PES_DEFICIENCIA_GRAU_ID { get; set; }

        [Display(Name = "Descrição")]
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

        [Display(Name = "% 1RM")]
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
        public int? emptyID { get; set; }
        public bool? predefined { get; set; }
        public int? GTTipoTreinoId { get; set; }
        public int? PEsId { get; set; }
        [Display(Name = "Periodização:")]
        public int? Periodizacao { get; set; }

        [Display(Name = "N° de Séries:")]
        public int? GT_Series_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Series_List { get; set; }

        [Display(Name = "N° de Repetições:")]
        public int? GT_Repeticoes_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Repeticoes_List { get; set; }

        [Display(Name = "% 1RM:")]
        public int? GT_Carga_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_Carga_List { get; set; }

        [Display(Name = "Tempo Descanso:")]
        public int? GT_TempoDescanso_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TempoDescanso_List { get; set; }
        [Display(Name = "Fases de treino:")]
        public int? FaseTreinoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FaseTreinoList { get; set; }
        [Display(Name = "Nome do treino:")]
        public int? GTTreinoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GTTreinoList { get; set; }

        [Display(Name = "Duração (Min.):")]
        public int? GT_DuracaoTreinoCardio_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_DuracaoTreinoCardioList { get; set; }

        public List<ExerciseArq> ExerciseArqList { get; set; }
        public List<ExerciseArq> ExerciseArqListTreino { get; set; }

        //[Required]
        [Display(Name = "Data de início do plano:")]
        [DataType(DataType.Text)]
        public string DateIni { get; set; }

        [Display(Name = "Data de fim:")]
        [DataType(DataType.Text)]
        public string DateEnd { get; set; }
        [Display(Name = "RM")]
        public string RM { get; set; }

        [Display(Name = "Carga")]
        public decimal? CargaUsada { get; set; }

        [Display(Name = "Rep's:")]
        public int? Reps { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> RepList { get; set; }

        [Display(Name = "Nome:")]
        public string Nome { get; set; }

        [Display(Name = "Observações:")]
        public string Observacoes { get; set; }

        [Display(Name = "FC (Min/Máx) bpm:")]
        public int? FC { get; set; }

        [Display(Name = "Nível / Resist. / Velocidade:")]
        public string Nivel { get; set; }

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
        public decimal? CARGA_USADA { get; set; }
        public decimal? ONERM { get; set; }
        public int? GT_DuracaoTreinoCardio_ID { get; set; }
        public int? FC { get; set; }
        public decimal? Nivel { get; set; }
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
        public int? q12 { get; set; }
        public int? q13 { get; set; }
        public int? q14 { get; set; }
        public int? Summary { get; set; }
        public string SummaryDesc { get; set; }
    }
    public class GT_Quest_SelfConcept
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
        public int? q12 { get; set; }
        public int? q13 { get; set; }
        public int? q14 { get; set; }
        public int? q15 { get; set; }
        public int? q16 { get; set; }
        public int? q17 { get; set; }
        public int? q18 { get; set; }
        public int? q19 { get; set; }
        public int? q20 { get; set; }
        public int? Summary { get; set; }
        public string SummaryDesc { get; set; }
    }
    public class CoronaryRisk
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        public string IdadeQuery { get; set; }
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
        public int? q12 { get; set; }
        public int? q13 { get; set; }
        public int? q14 { get; set; }
        public int? q15 { get; set; }
        public int? q16 { get; set; }
        public int? Summary { get; set; }
        public string SummaryDesc { get; set; }
        public int? txtCigarrosMedia { get; set; }
        public int? txtMaxSistolica { get; set; }
        public int? txtMinSistolica { get; set; }
        public int? txtMaxDistolica { get; set; }
        public int? txtMinDistolica { get; set; }
        public string txtMedicamento { get; set; }
        public int? txtGlicose1 { get; set; }
        public int? txtGlicose2 { get; set; }
        public int? txtPerimetro { get; set; }
        public int? txtIMC { get; set; }
        public string txtCardiaca { get; set; }
        public string txtVascular { get; set; }
        public string txtCerebroVascular { get; set; }
        public string txtCardioVascularOutras { get; set; }
        public string txtObstrucao { get; set; }
        public string txtAsma { get; set; }
        public string txtFibrose { get; set; }
        public string txtPulmomarOutras { get; set; }
        public string txtDiabetes1 { get; set; }
        public string txtDiabetes2 { get; set; }
        public string txtTiroide { get; set; }
        public string txtRenais { get; set; }
        public string txtFigado { get; set; }
        public string txtMetabolicaOutras { get; set; }

        [Display(Name = "Dor, desconforto no peito, pescoço, queixo, braços ou áreas, que possa ser devido a isquémia (falta de irrigação sanguínea)")]
        public bool chkDor { get; set; }

        [Display(Name = "Respiração curta em repouso ou em actividade de média intensidade")]
        public bool chkRespiracao { get; set; }

        [Display(Name = "Tonturas ou síncope (desamaio)")]
        public bool chkTonturas { get; set; }

        [Display(Name = "Dispeneia nocturna (ressonar)")]
        public bool chkDispeneia { get; set; }

        [Display(Name = "Edema no tornozelo")]
        public bool chkEdema { get; set; }

        [Display(Name = "Palpitações (ritmo anormalmente rápido ou irregular) e taquicárdia (ritmo cardíaco anormalmente acelerado)")]
        public bool chkPalpitacoes { get; set; }

        [Display(Name = "Claudicação intermitente (coxear ocasional, acompanhado de dores nas pernas, vulgarmente causado por doença arterial)")]
        public bool chkClaudicacao { get; set; }

        [Display(Name = "Murmúrio no coração")]
        public bool chkMurmurio { get; set; }

        [Display(Name = "Fadiga invulgar")]
        public bool chkfadiga { get; set; }
        //
        [Display(Name = "Cardíaca")]
        public bool chkCardiaca { get; set; }
        [Display(Name = "Vascular Periférica")]
        public bool chkVascular { get; set; }
        [Display(Name = "Cerebrovascular")]
        public bool chkCerebroVascular { get; set; }
        [Display(Name = "Outras")]
        public bool chkCardioVascularOutras { get; set; }
        [Display(Name = "Obstrução pulmonar crónica")]
        public bool chkObstrucao { get; set; }
        [Display(Name = "Asma")]
        public bool chkAsma { get; set; }
        [Display(Name = "Fibrose quística")]
        public bool chkFibrose { get; set; }
        [Display(Name = "Outras")]
        public bool chkPulmomarOutras { get; set; }
        [Display(Name = "Diabetes Tipo I")]
        public bool chkDiabetes1 { get; set; }
        [Display(Name = "Diabetes Tipo II")]
        public bool chkDiabetes2 { get; set; }
        [Display(Name = "Problemas de tiróide")]
        public bool chkTiroide { get; set; }
        [Display(Name = "Doenças renais")]
        public bool chkRenais { get; set; }
        [Display(Name = "Doenças de fígado")]
        public bool chkFigado { get; set; }
        [Display(Name = "Outras")]
        public bool chkMetabolicaOutras { get; set; }
    }

    public class Health
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        public int? q1 { get; set; }
        public int? q2 { get; set; }
        public int? q3 { get; set; }
        public int? q4 { get; set; }
        public int? q5 { get; set; }
        public int? q5_1 { get; set; }
        public int? q5_2 { get; set; }
        public int? q5_3 { get; set; }
        public int? q6 { get; set; }
        public int? q7 { get; set; }
        public int? q8 { get; set; }
        public int? q9 { get; set; }
        public int? q10 { get; set; }
        public int? q11 { get; set; }
        public int? q12 { get; set; }
        public int? q13 { get; set; }
        public int? q14 { get; set; }
        public int? q15 { get; set; }
        public int? q16 { get; set; }
        public int? q17 { get; set; }

        [Display(Name = "Início")]
        public string dtOsteoporoseI { get; set; }
        [Display(Name = "Fim")]
        public string dtOsteoporoseF { get; set; }
        [Display(Name = "Onde?")]
        public string txtOsteoporose { get; set; }
        //
        [Display(Name = "Início")]
        public string dtOsteoartoseI { get; set; }
        [Display(Name = "Fim")]
        public string dtOsteoartoseF { get; set; }
        [Display(Name = "Onde?")]
        public string txtOsteoartose { get; set; }
        //
        [Display(Name = "Início")]
        public string dtArticularesI { get; set; }
        [Display(Name = "Fim")]
        public string dtArticularesF { get; set; }
        [Display(Name = "Onde?")]
        public string txtArticulares { get; set; }
        //
        [Display(Name = "Início")]
        public string dtLesoesI { get; set; }
        [Display(Name = "Fim")]
        public string dtLesoesF { get; set; }
        [Display(Name = "Onde?")]
        public string txtLesoes { get; set; }
        //
        [Display(Name = "Início")]
        public string dtDorI { get; set; }
        [Display(Name = "Fim")]
        public string dtDorF { get; set; }
        [Display(Name = "Onde?")]
        public string txtDor { get; set; }
        [Display(Name = "Causa")]
        public string txtCausaDor { get; set; }
        [Display(Name = "Início")]
        public string dtEscolioseI { get; set; }
        [Display(Name = "Fim")]
        public string dtEscolioseF { get; set; }
        [Display(Name = "Início")]
        public string dtHiperlordoseI { get; set; }
        [Display(Name = "Fim")]
        public string dtHiperlordoseF { get; set; }
        [Display(Name = "Início")]
        public string dtHipercifoseI { get; set; }
        [Display(Name = "Fim")]
        public string dtHipercifoseF { get; set; }
        //
        [Display(Name = "Início")]
        public string dtJoelhoI { get; set; }
        [Display(Name = "Fim")]
        public string dtJoelhoF { get; set; }
        [Display(Name = "causa?")]
        public string txtJoelho { get; set; }
        //
        [Display(Name = "Início")]
        public string dtOmbroI { get; set; }
        [Display(Name = "Fim")]
        public string dtOmbroF { get; set; }
        [Display(Name = "causa?")]
        public string txtOmbro { get; set; }
        //
        [Display(Name = "Início")]
        public string dtPunhoI { get; set; }
        [Display(Name = "Fim")]
        public string dtPunhoF { get; set; }
        [Display(Name = "causa?")]
        public string txtPunho { get; set; }
        //
        [Display(Name = "Início")]
        public string dtTornozeloI { get; set; }
        [Display(Name = "Fim")]
        public string dtTornozeloF { get; set; }
        [Display(Name = "causa?")]
        public string txtTornozelo { get; set; }
        //
        [Display(Name = "Início")]
        public string dtOutraArticI { get; set; }
        [Display(Name = "Fim")]
        public string dtOutraArticF { get; set; }
        [Display(Name = "1.Qual?")]
        public string txtOutraArtic1 { get; set; }
        [Display(Name = "2.Qual?")]
        public string txtOutraArtic2 { get; set; }
        //
        [Display(Name = "Início")]
        public string dtParkinsonI { get; set; }
        //
        [Display(Name = "Início")]
        public string dtVisualI { get; set; }
        [Display(Name = "Fim")]
        public string dtVisualF { get; set; }
        [Display(Name = "Tipo?")]
        public string txtVisual { get; set; }
        //
        [Display(Name = "Início")]
        public string dtAuditivoI { get; set; }
        [Display(Name = "Fim")]
        public string dtAuditivoF { get; set; }
        [Display(Name = "Tipo?")]
        public string txtAuditivo { get; set; }
        //
        [Display(Name = "Início")]
        public string dtGastroI { get; set; }
        [Display(Name = "Fim")]
        public string dtGastroF { get; set; }
        [Display(Name = "Tipo?")]
        public string txtGastro { get; set; }
        //
        [Display(Name = "Idade")]
        public int? txtCirugiaIdade1 { get; set; }
        [Display(Name = "Onde?")]
        public string txtCirugiaOnde1 { get; set; }
        [Display(Name = "Causa?")]
        public string txtCirugiaCausa1 { get; set; }
        [Display(Name = "Restrições?")]
        public string txtCirugiaRestricao1 { get; set; }
        [Display(Name = "Idade")]
        public int? txtCirugiaIdade2 { get; set; }
        [Display(Name = "Onde?")]
        public string txtCirugiaOnde2 { get; set; }
        [Display(Name = "Causa?")]
        public string txtCirugiaCausa2 { get; set; }
        [Display(Name = "Restrições?")]
        public string txtCirugiaRestricao2 { get; set; }
        //
        [Display(Name = "Quais?")]
        public string txtProbSaude { get; set; }
        //
        [Display(Name = "Qual?")]
        public string txtInactividade { get; set; }
    }

    public class Flexibility
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        [Display(Name = "Teste:")]
        public int TipoId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> TipoList { get; set; }
        public int? iFlexiAct { get; set; }
        public string lblResActualFlexi { get; set; }
        public int? iFlexiAnt { get; set; }
        public string lblResAnteriorFlexi { get; set; }
        //
        public int? TENTATIVA1 { get; set; }
        public int? TENTATIVA2 { get; set; }
        public int? ESPERADO { get; set; }
        public int? RESULTADO { get; set; }
    }

    public class BodyComposition
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        
        [Display(Name = "Nível Actividade Diária:")]
        public int GT_TipoNivelActividade_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoNivelActividade_List { get; set; }

        [Display(Name = "Método:")]
        public int GT_TipoMetodoComposicao_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoMetodoComposicao_List { get; set; }

        [Display(Name = "Protocolo:")]
        public int GT_TipoTesteComposicao_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoTesteComposicao_List { get; set; }

        //[Display(Name = "Peso:")]
        //public string Peso { get; set; }
        [Display(Name = "Perímetro:")]
        public int? Perimetro { get; set; }
        [Display(Name = "Actual:")]
        public string Actual { get; set; }
        [Display(Name = "Abdominal:")]
        public int? Abdominal { get; set; }
        [Display(Name = "Desejável:")]
        public string Desejavel { get; set; }
        [Display(Name = "Cintura:")]
        public int? Cintura { get; set; }
        [Display(Name = "A perder:")]
        public decimal? Aperder { get; set; }
        //[Display(Name = "Circunferências:")]
        //public int? Circunferencia { get; set; }
        [Display(Name = "Perímetro entre a apêndice xifóide e umbigo?:")]
        public int? PerimetroUmbigo { get; set; }
        [Display(Name = "Resistência:")]
        public int? Resistencia { get; set; }
        [Display(Name = "Pregas:")]
        public int? Pregas { get; set; }
        [Display(Name = "Peitoral:")]
        public int? Peitoral { get; set; }
        [Display(Name = "Tricipital:")]
        public int? Tricipital { get; set; }
        [Display(Name = "Subescapular:")]
        public int? Subescapular { get; set; }
        [Display(Name = "%MG:")]
        public decimal? PercMG { get; set; }
        [Display(Name = "MIG:")]
        public decimal? MIG { get; set; }
        [Display(Name = "MG:")]
        public decimal? MG { get; set; }
        [Display(Name = "IMC:")]
        public int? IMC { get; set; }

        [Display(Name = "Metabolismo de Repouso:")]
        public decimal? MetabolismoRepouso { get; set; }
        [Display(Name = "Estimação:")]
        public decimal? Estimacao { get; set; }
        [Display(Name = "% MG Desejável:")]
        public string PercMGDesejavel { get; set; }

        [Display(Name = "Tricipital:")]
        public int? TricipitalFem { get; set; }
        [Display(Name = "SupraIlíaca:")]
        public int? SupraIliacaFem { get; set; }
        [Display(Name = "Abdominal:")]
        public int? AbdominalFem { get; set; }
        public int? iFlexiAct { get; set; }
        public string lblResActualFlexi { get; set; }
        public int? iFlexiAnt { get; set; }
        public string lblResAnteriorFlexi { get; set; }
        public DateTime? lblDataInsercao { get; set; }
    }
    public class Cardio
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }

        [Display(Name = "Método:")]
        public int? GT_TipoMetodoComposicao_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoMetodoComposicao_List { get; set; }

        [Display(Name = "Protocolo:")]
        public int? GT_TipoTesteCardio_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoTesteCardio_List { get; set; }

        public decimal? V02Mets { get; set; }
        public decimal? V02CustoCalMin { get; set; }

        [Display(Name = "VO2 máx.:")]
        public decimal? V02max { get; set; }
        [Display(Name = "VO2 Desejável:")]
        public string V02desejavel { get; set; }

        [Display(Name = "Custo calórico do exercicío:")]
        public decimal? CustoCalorico { get; set; }

        [Display(Name = "Tempo de realização dos 2000 M:")]
        public decimal? TempoRealizacao200 { get; set; }

        [Display(Name = "Média Watts:")]
        public decimal? MediaWatts { get; set; }

        [Display(Name = "Distância percorrida em 12 m:")]
        public decimal? Distancia12m { get; set; }

        [Display(Name = "Tempo a percorrer 1600 m:")]
        public decimal? Tempo1600m { get; set; }

        [Display(Name = "Frequência cardíaca no últimos 400m:")]
        public decimal? Frequencia400m { get; set; }

        [Display(Name = "Frequência cardíaca no fim do teste:")]
        public decimal? FrequenciaFimTeste { get; set; }

        [Display(Name = "Média da frequência cardíaca:")]
        public decimal? MediaFrequencia { get; set; }

        [Display(Name = "FC 15 seg. depois do teste:")]
        public decimal? FC15sec { get; set; }

        [Display(Name = "FC aos 3 minutos:")]
        public decimal? FC3min { get; set; }

        [Display(Name = "Velocidade:")]
        public decimal? Velocidade { get; set; }

        [Display(Name = "Velocidade:")]
        public decimal? VelocidadeMPH { get; set; }

        [Display(Name = "Factor de correcção:")]
        public int? FactorCorrecao { get; set; }

        [Display(Name = "Carga:")]
        public decimal? Carga { get; set; }

        [Display(Name = "FC 4º minuto:")]
        public decimal? FC4min { get; set; }

        [Display(Name = "FC 5º minuto:")]
        public decimal? FC5min { get; set; }

        [Display(Name = "Valor Médio FC:")]
        public decimal? ValorMedioFC { get; set; }

        [Display(Name = "VO2 Carga:")]
        public decimal? VO2Carga { get; set; }

        [Display(Name = "Equação da Recta:")]
        public decimal? EquacaoRecta { get; set; }
        public int? iFlexiAct { get; set; }
        public string lblResActualFlexi { get; set; }
        public int? iFlexiAnt { get; set; }
        public string lblResAnteriorFlexi { get; set; }
        public DateTime? lblDataInsercao { get; set; }
        public decimal? V02maxResp { get; set; }

        public decimal? YMCACarga1 { get; set; }
        public decimal? YMCACarga2 { get; set; }
        public decimal? YMCACarga3 { get; set; }
        public decimal? YMCACarga4 { get; set; }
        public decimal? YMCATrab1{ get; set; }
        public decimal? YMCATrab2 { get; set; }
        public decimal? YMCATrab3 { get; set; }
        public decimal? YMCATrab4 { get; set; }
        public decimal? YMCAPot1 { get; set; }
        public decimal? YMCAPot2 { get; set; }
        public decimal? YMCAPot3 { get; set; }
        public decimal? YMCAPot4 { get; set; }
        public decimal? YMCAVO21 { get; set; }
        public decimal? YMCAVO22 { get; set; }
        public decimal? YMCAVO23 { get; set; }
        public decimal? YMCAVO24 { get; set; }
        public decimal? YMCAFC1 { get; set; }
        public decimal? YMCAFC2 { get; set; }
        public decimal? YMCAFC3 { get; set; }
        public decimal? YMCAFC4 { get; set; }
    }

    public class Elderly
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }

        [Display(Name = "Teste:")]
        public int GT_TipoTestePessoaIdosa_ID { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoTestePessoaIdosa_List { get; set; }

        [Display(Name = "Desejável:")]
        public string Desejavel { get; set; }

        [Display(Name = "Nº Elevações:")]
        public decimal? NElevacoes { get; set; }

        [Display(Name = "Nº Flexões:")]
        public decimal? NFlexoes { get; set; }

        [Display(Name = "Distância:")]
        public decimal? DistanciaSentarAlcancar { get; set; }

        [Display(Name = "Tempo:")]
        public decimal? TempoAgilidade { get; set; }

        [Display(Name = "Distância:")]
        public decimal? DistanciaAlcancar { get; set; }

        [Display(Name = "Distância:")]
        public decimal? DistanciaAndar { get; set; }

        [Display(Name = "Nº Subidas Step:")]
        public decimal? SubidasStep { get; set; }

        [Display(Name = "Peso Desejável:")]
        public decimal? PesoDesejavel { get; set; }

        [Display(Name = "IMC:")]
        public int? IMC { get; set; }

        [Display(Name = "% MG:")]
        public decimal? MG { get; set; }

        [Display(Name = "% MG Desejável:")]
        public string MGDesejavel { get; set; }

        public decimal? Valor { get; set; }
        public int? iFlexiAct { get; set; }
        public string lblResActualFlexi { get; set; }
        public int? iFlexiAnt { get; set; }
        public string lblResAnteriorFlexi { get; set; }
        public DateTime? lblDataInsercao { get; set; }
    }
    public class Force
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }

        [Display(Name = "Teste:")]
        public int GT_TipoTesteForca_ID { get; set; }
        
        public IEnumerable<System.Web.Mvc.SelectListItem> GT_TipoTesteForca_List { get; set; }

        [Display(Name = "Nº. Abdominais:")]
        public int? NAbdominais { get; set; }

        [Display(Name = "Nº Flexões:")]
        public int? NFlexoes { get; set; }

        [Display(Name = "Carga Utilizada:")]
        public decimal? CargaBraco { get; set; }

        [Display(Name = "Razão:")]
        public decimal? RazaoBraco { get; set; }

        [Display(Name = "Défice de Força:")]
        public string DeficeBraco { get; set; }

        [Display(Name = "90% RM:")]
        public decimal? NoventaRMBraco { get; set; }

        [Display(Name = "Nº Reps a 90%:")]
        public int? NoventaRepsBraco { get; set; }

        [Display(Name = "Tipo de Trabalho a Desenvolver:")]
        public string TrabalhoDesenvolverBraco { get; set; }
        //

        [Display(Name = "Carga Utilizada:")]
        public decimal? CargaPerna { get; set; }

        [Display(Name = "Razão:")]
        public decimal? RazaoPerna { get; set; }

        [Display(Name = "Défice de Força:")]
        public string DeficePerna { get; set; }

        [Display(Name = "90% RM:")]
        public decimal? NoventaRMPerna { get; set; }

        [Display(Name = "Nº Reps a 90%:")]
        public int? NoventaRepsPerna { get; set; }

        [Display(Name = "Tipo de Trabalho a Desenvolver:")]
        public string TrabalhoDesenvolverPerna { get; set; }

        [Display(Name = "Resultado:")]
        public string ResultadoVLinear { get; set; }

        [Display(Name = "1ª Tentativa:")]
        public decimal? PrimeraTentativaVLinear { get; set; }

        [Display(Name = "2ª Tentativa:")]
        public decimal? SegundaTentativaVLinear { get; set; }

        [Display(Name = "3ª Tentativa:")]
        public decimal? TerceiraTentativaVLinear { get; set; }


        [Display(Name = "1ª Tentativa:")]
        public decimal? PrimeraTentativaVResist { get; set; }

        [Display(Name = "2ª Tentativa:")]
        public decimal? SegundaTentativaVResist { get; set; }

        [Display(Name = "3ª Tentativa:")]
        public decimal? TerceiraTentativaVResist { get; set; }

        [Display(Name = "4ª Tentativa:")]
        public decimal? QuartaTentativaVResist { get; set; }

        [Display(Name = "5ª Tentativa:")]
        public decimal? QuintaTentativaVResist { get; set; }

        [Display(Name = "6ª Tentativa:")]
        public decimal? SextaTentativaVResist { get; set; }

        [Display(Name = "7ª Tentativa:")]
        public decimal? SetimaTentativaVResist { get; set; }

        [Display(Name = "8ª Tentativa:")]
        public decimal? OitavaTentativaVResist { get; set; }

        [Display(Name = "9ª Tentativa:")]
        public decimal? NonaTentativaVResist { get; set; }

        [Display(Name = "10ª Tentativa:")]
        public decimal? DecimaTentativaVResist { get; set; }

        [Display(Name = "Fadiga de Sprint:")]
        public decimal? sprintVResist { get; set; }
        [Display(Name = "Capacidade de manter a fadiga:")]
        public decimal? capacidadeVResist { get; set; }


        [Display(Name = "1ª Tentativa:")]
        public decimal? PrimeraTentativaAgilidade { get; set; }

        [Display(Name = "2ª Tentativa:")]
        public decimal? SegundaTentativaAgilidade { get; set; }

        [Display(Name = "3ª Tentativa:")]
        public decimal? TerceiraTentativaAgilidade { get; set; }

        [Display(Name = "Resultado:")]
        public decimal? ResultadoAgilidade { get; set; }

        [Display(Name = "1ª Tentativa:")]
        public decimal? PrimeraTentativaExpH { get; set; }

        [Display(Name = "2ª Tentativa:")]
        public decimal? SegundaTentativaExpH { get; set; }

        [Display(Name = "3ª Tentativa:")]
        public decimal? TerceiraTentativaExpH { get; set; }

        [Display(Name = "Resultado:")]
        public decimal? ResultadoExpH { get; set; }

        [Display(Name = "1ª Tentativa:")]
        public decimal? PrimeraTentativaExpV { get; set; }

        [Display(Name = "2ª Tentativa:")]
        public decimal? SegundaTentativaExpV { get; set; }

        [Display(Name = "3ª Tentativa:")]
        public decimal? TerceiraTentativaExpV { get; set; }

        [Display(Name = "Resultado:")]
        public decimal? ResultadoExpV { get; set; }

        [Display(Name = "Valor Inicial:")]
        public decimal? ValorInitExpV { get; set; }

        [Display(Name = "Potênc. Muscular:")]
        public decimal? PotenciaExpV { get; set; }


        [Display(Name = "Desejável:")]
        public string DesejavelAbdominais { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelFlexoes { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelBracos { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelPerna { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelVLinear { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelVResist { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelAgilidade { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelExpH { get; set; }

        [Display(Name = "Desejável:")]
        public string DesejavelExpV { get; set; }

        public decimal? Valor { get; set; }
        public int? iFlexiAct { get; set; }
        public string lblResActualFlexi { get; set; }
        public decimal? iFlexiAnt { get; set; }
        public string lblResAnteriorFlexi { get; set; }
        public DateTime? lblDataInsercao { get; set; }
    }

    public class Functional
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        [Display(Name = "Tipo do Desporto:")]
        public string Desporto { get; set; }
        [Display(Name = "Posição a que Joga:")]
        public string Posicao { get; set; }
        [Display(Name = "Mão Dominante:")]
        public int? Mao { get; set; }
        [Display(Name = "Perna Dominante:")]
        public int? Perna { get; set; }
        [Display(Name = "Olho Dominante:")]
        public int? Olho { get; set; }
        public int? Resultado { get; set; }
        public int? RESP_01 { get; set; }
        public int? RESP_02 { get; set; }
        public int? RESP_03 { get; set; }
        public int? RESP_04 { get; set; }
        public int? RESP_05 { get; set; }
        public int? RESP_06 { get; set; }
        public int? RESP_07 { get; set; }
        public DateTime? lblDataInsercao { get; set; }
    }
    public class Search
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        public int? Pescription { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Pescription_List { get; set; }
    }
    public class MediumWeight
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        public string PercFemale { get; set; }
        public string PercMale { get; set; }
        public string PercBothGender { get; set; }
    }
    public class Others
    {
        public int? ID { get; set; }
        public int? PEsId { get; set; }
        public int? AnsiedadeDepressaototalAtletas { get; set; }
        public double? AnsiedadeDepressaoPercAtletas { get; set; }
        public int? AnsiedadeDepressaoTotalHomens { get; set; }
        public double? AnsiedadeDepressaoPercHomens { get; set; }
        public int? AnsiedadeDepressaoTotalMulheres { get; set; }
        public double? AnsiedadeDepressaoPercMulheres { get; set; }
        public int? AnsiedadeDepressaoTotalAvaliacoes { get; set; }

        public int? RespAutoConceitototalAtletas { get; set; }
        public double? RespAutoConceitoPercAtletas { get; set; }
        public int? RespAutoConceitoTotalHomens { get; set; }
        public double? RespAutoConceitoPercHomens { get; set; }
        public int? RespAutoConceitoTotalMulheres { get; set; }
        public double? RespAutoConceitoPercMulheres { get; set; }
        public int? RespAutoConceitoTotalAvaliacoes { get; set; }

        public int? RespRiscototalAtletas { get; set; }
        public double? RespRiscoPercAtletas { get; set; }
        public int? RespRiscoTotalHomens { get; set; }
        public double? RespRiscoPercHomens { get; set; }
        public int? RespRiscoTotalMulheres { get; set; }
        public double? RespRiscoPercMulheres { get; set; }
        public int? RespRiscoTotalAvaliacoes { get; set; }

        public List<string> AnsiedadeDepressaosValue { get; set; }
        public List<double?> AnsiedadeDepressaoValue { get; set; }
    }

public class OthersGraph
    {
        public string name;
        public double? data;
    }

}