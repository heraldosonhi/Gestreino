using System.Linq;

namespace Gestreino.Classes
{
    public class Configs
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();

        public static int? INST_INSTITUICAO_ID = 1;  
        public static string INST_INSTITUICAO_SIGLA ;
        public static string INST_INSTITUICAO_NOME;
        public static string INST_INSTITUICAO_ENDERECO;
        public static string INST_INSTITUICAO_URL;
        public static string INST_INSTITUICAO_LOGO;
        public static string INST_PER_TEMA_1;
        public static string INST_PER_TEMA_1_SIDEBAR;
        public static string INST_PER_TEMA_2;
        public static int? INST_PER_LOGOTIPO_WIDTH;
        public static int? INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY=11;
        public static int? GT_EXERCISE_TYPE_BODYMASS = 1;
        public static int? GT_EXERCISE_TYPE_CARDIO = 3;
        public static int[] GT_EXERCISE_TYPE_CARDIO_INCLINACAO = { 5,6 };
        public static int? GT_EXERCISE_TYPE_BODYMASS_EX_MAX_ALLOWED = 12;
        public static int? GT_EXERCISE_TYPE_CARDIO_EX_MAX_ALLOWED = 4;
        public static int? INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO=2;
        public static string GESTREINO_AVALIDO_NOME;
        public static string GESTREINO_AVALIDO_IDADE;
        public static string GESTREINO_AVALIDO_PESO;
        public static string GESTREINO_AVALIDO_ALTURA;
        public static string GESTREINO_AVALIDO_SEXO;
        public static int? INST_MDL_GPAG_MOEDA_PADRAO;
        public static string INST_MDL_GPAG_MOEDA_PADRAO_CODIGO;
        public static string INST_MDL_GPAG_MOEDA_PADRAO_SIGLA;
        public static int? INST_MDL_GPAG_NUMERO_COPIAS_RECIBO;
        public static int? INST_MDL_GPAG_EMOL_DATA_LIMITE; 
        public static int? INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS;
        public static int? INST_MDL_GPAG_NOTA_DECIMAL;
        public static int? INST_MDL_GP_BI_MAXLENGTH;
        public static string NET_LDAP_BASE;
        public static string NET_LDAP_HOSTNAME;
        public static string NET_ENDERECO_IP_INTERNO;
        public static string NET_ENDERECO_IP_EXTERNO;
        public static string NET_STMP_HOST;
        public static int? NET_STMP_PORT;
        public static string NET_SMTP_USERNAME;
        public static string NET_SMTP_SENHA;
        public static string NET_STMP_FROM;
        public static int? SEC_SENHA_TENT_BLOQUEIO;
        public static int? SEC_SENHA_TENT_BLOQUEIO_TEMPO;
        public static int? SEC_SENHA_RECU_LIMITE_EMAIL;
        public static int? SEC_SESSAO_TIMEOUT_TEMPO;
        public static string API_CURRENCY_TOKEN;
        public static string API_CURRENCY_BASE;
        public static string API_SMS_TOKEN;
        public static string API_SMS_BASE;
        public static bool? API_SMS_ACTIVO;
        public static string API_SMS_SENDER_ID;
        public static int[] TOKENS = { 1 };

        public void BeginConfig()
        {
            if (string.IsNullOrEmpty(Configs.INST_INSTITUICAO_SIGLA))
            {
                var configvalues = databaseManager.GRL_DEFINICOES.Join(databaseManager.INST_APLICACAO,x => x.INST_APLICACAO_ID, y => y.ID,(x, y) => new { x, y }).Where(y => y.y.ID == INST_INSTITUICAO_ID).ToList();
                
                INST_INSTITUICAO_SIGLA = configvalues[0].y.SIGLA;
                INST_INSTITUICAO_NOME = configvalues[0].y.NOME;
                INST_PER_TEMA_1 = configvalues[0].x.INST_PER_TEMA_1;
                INST_PER_TEMA_1_SIDEBAR = configvalues[0].x.INST_PER_TEMA_1_SIDEBAR;
                INST_PER_TEMA_2 = configvalues[0].x.INST_PER_TEMA_2;
                INST_PER_LOGOTIPO_WIDTH = configvalues[0].x.INST_PER_LOGOTIPO_WIDTH;
                INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS = configvalues[0].x.INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS;
                INST_MDL_GPAG_NOTA_DECIMAL = configvalues[0].x.INST_MDL_GPAG_NOTA_DECIMAL;
                NET_STMP_HOST = configvalues[0].x.NET_STMP_HOST;
                NET_STMP_PORT = configvalues[0].x.NET_STMP_PORT;
                NET_SMTP_USERNAME = configvalues[0].x.NET_SMTP_USERNAME;
                NET_SMTP_SENHA = configvalues[0].x.NET_SMTP_SENHA;
                NET_STMP_FROM = configvalues[0].x.NET_SMTP_FROM;
                SEC_SENHA_TENT_BLOQUEIO = configvalues[0].x.SEC_SENHA_TENT_BLOQUEIO;
                SEC_SENHA_TENT_BLOQUEIO_TEMPO = configvalues[0].x.SEC_SENHA_TENT_BLOQUEIO_TEMPO;
                SEC_SENHA_RECU_LIMITE_EMAIL = configvalues[0].x.SEC_SENHA_RECU_LIMITE_EMAIL;
                SEC_SESSAO_TIMEOUT_TEMPO = configvalues[0].x.SEC_SESSAO_TIMEOUT_TEMPO;
                
            }
        }
    }
}