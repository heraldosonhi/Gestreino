using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Gestreino.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using static Gestreino.Classes.SelectValues;

namespace Gestreino.Classes
{
    public class Configs
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();

        // INST
        public static int? INST_INSTITUICAO_ID = 1; //APLICACAO ID 1 
        public static string INST_INSTITUICAO_SIGLA ;
        public static string INST_INSTITUICAO_NOME;
        public static string INST_INSTITUICAO_ENDERECO;
        public static string INST_INSTITUICAO_URL;
        // PERSONALIZE
        public static string INST_PER_TEMA_1;
        public static string INST_PER_TEMA_1_SIDEBAR;
        public static string INST_PER_TEMA_2;
        public static int? INST_PER_LOGOTIPO_WIDTH;

        public static int? INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY=11;

        public static int? GT_EXERCISE_TYPE_BODYMASS = 1;//"Musculação";
        public static int? GT_EXERCISE_TYPE_CARDIO = 3;//"Cardiovascular";
        public static int? INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO=2;

        public static string GESTREINO_AVALIDO_NOME;
        public static string GESTREINO_AVALIDO_IDADE;
        public static string GESTREINO_AVALIDO_PESO;
        public static string GESTREINO_AVALIDO_ALTURA;
        public static string GESTREINO_AVALIDO_SEXO;

        // ADM
        /*
        public static int? INST_MDL_ADM_VLRID_MODULO_CAND;
        public static int? INST_MDL_ADM_VLRID_MODULO_PORTAL;
        public static int? INST_MDL_ADM_VLRID_MODULO_ADM;
        public static int? INST_MDL_ADM_VLRID_MODULO_GA;
        public static int? INST_MDL_ADM_VLRID_MODULO_GPAG;
        public static int? INST_MDL_ADM_VLRID_MODULO_GP;
        public static int? INST_MDL_ADM_VLRID_MODULO_RPT;
        public static int? INST_MDL_ADM_VLRID_MODULO_HR;
        public static int? INST_MDL_ADM_VLRID_MODULO_PD;
        public static int? INST_MDL_ADM_VLRID_MODULO_BIB;
        public static int? INST_MDL_ADM_VLRID_MODULO_PWBI;

        //--
        public static string INST_MDL_ADM_MODULO_CAND = String.Empty;
        public static string INST_MDL_ADM_MODULO_PORTAL = String.Empty;
        public static string INST_MDL_ADM_MODULO_ADM = String.Empty;
        public static string INST_MDL_ADM_MODULO_GA = String.Empty;
        public static string INST_MDL_ADM_MODULO_GPAG = String.Empty;
        public static string INST_MDL_ADM_MODULO_GP = String.Empty;
        public static string INST_MDL_ADM_MODULO_RPT = String.Empty;
        public static string INST_MDL_ADM_MODULO_HR = String.Empty;
        public static string INST_MDL_ADM_MODULO_PD = String.Empty;
        public static string INST_MDL_ADM_MODULO_BIB = String.Empty;
        public static string INST_MDL_ADM_MODULO_PWBI = String.Empty;
        //--
        public static int? INST_MDL_ADM_VLRID_ESTADO_AP;
        public static int? INST_MDL_ADM_VLRID_ESTADO_INSCRITO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_ADMITIDO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_NAO_ADMITIDO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_ANULADO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_MATRICULADO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_NORMAL;
        public static int? INST_MDL_ADM_VLRID_ESTADO_EDICAO; 
        public static int? INST_MDL_ADM_VLRID_ESTADO_FINALIZADO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_PUBLICADO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_CONCLUIDO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_FREQUENTAR;
        public static int? INST_MDL_ADM_VLRID_ESTADO_MUDANCA_CURSO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_AN_MATRICULA;
        public static int? INST_MDL_ADM_VLRID_ESTADO_SU_MATRICULA;
        public static int? INST_MDL_ADM_VLRID_ESTADO_AV;
        public static int? INST_MDL_ADM_VLRID_ESTADO_NV;
        public static int? INST_MDL_ADM_VLRID_ESTADO_CRIACAO;
        public static int? INST_MDL_ADM_VLRID_ESTADO_VALIDADA;
        public static int? INST_MDL_ADM_VLRID_ESTADO_APU;
        public static int? INST_MDL_ADM_VLRID_ESTADO_EXP;

        public static int? INST_MDL_ADM_VLRID_RESIDENCIA_PROPIA;
        public static int? INST_MDL_ADM_VLRID_PROFISSAO_ESTUDANTE;
        public static int? INST_MDL_ADM_VLRID_REGIME_PROFISSIONAL;
        public static int? INST_MDL_ADM_VLRID_REGIME_TRANSFERENCIA;
        public static int? INST_MDL_ADM_VLRID_UTILIZADOR_CAND_GERAL;
        public static int? INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_TMP;
        public static int? INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_ALUNO;
        public static int? INST_MDL_ADM_VLRID_ESTUDO_PROCEDENCIA_ESTRANGEIRO;
        public static int? INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY;
        public static int? INST_MDL_ADM_VLRID_ARQUIVO_FOTOGRAFIA;
        public static int? INST_MDL_ADM_VLRID_TIPODOC_BI;
        public static int? INST_MDL_ADM_VLRID_PAUTAS_OBSERVACOES_FALTOU;
        // GA
        public static int? INST_MDL_GA_CARACT_ESP_PARAM;
        public static int? INST_MDL_GA_PLANOS_ESTUDO_ANOS_ACADEMICOS;
        public static decimal? INST_MDL_GA_PLANO_OCORRENCIA_BASE_FORMULA;
        */
        // GPAG
        public static int? INST_MDL_GPAG_MOEDA_PADRAO;
        //-
        public static string INST_MDL_GPAG_MOEDA_PADRAO_CODIGO;
        public static string INST_MDL_GPAG_MOEDA_PADRAO_SIGLA;
        //-
        public static int? INST_MDL_GPAG_NUMERO_COPIAS_RECIBO;
        public static int? INST_MDL_GPAG_EMOL_DATA_LIMITE; 
        public static int? INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS;
        public static int? INST_MDL_GPAG_NOTA_DECIMAL;
        // G/P
        public static int? INST_MDL_GP_BI_MAXLENGTH;
        // NETWORK
        public static string NET_LDAP_BASE;
        public static string NET_LDAP_HOSTNAME;
        public static string NET_ENDERECO_IP_INTERNO;
        public static string NET_ENDERECO_IP_EXTERNO;
        public static string NET_STMP_HOST;
        public static int? NET_STMP_PORT;
        public static string NET_SMTP_USERNAME;
        public static string NET_SMTP_SENHA;
        public static string NET_STMP_FROM;
        // SECURITY
        public static int? SEC_SENHA_TENT_BLOQUEIO;
        public static int? SEC_SENHA_TENT_BLOQUEIO_TEMPO;
        public static int? SEC_SENHA_RECU_LIMITE_EMAIL;
        public static int? SEC_SESSAO_TIMEOUT_TEMPO;
        // API
        public static string API_CURRENCY_TOKEN;
        public static string API_CURRENCY_BASE;
        public static string API_SMS_TOKEN;
        public static string API_SMS_BASE;
        public static bool? API_SMS_ACTIVO;
        public static string API_SMS_SENDER_ID;
        // ANO LECTIVO
        public static int GRL_A_LECTIVO;


        // GRL and TMP
        public static int[] TOKENS = { 1 }; // 0 => Recuperacao da Senha de acesso

    


        public void BeginConfig()
        {
            if (string.IsNullOrEmpty(Configs.INST_INSTITUICAO_SIGLA))
            {
                // Setup Static Config Values 
                var configvalues = databaseManager.GRL_DEFINICOES.Join(databaseManager.INST_APLICACAO,x => x.INST_APLICACAO_ID, y => y.ID,(x, y) => new { x, y }).Where(y => y.y.ID == INST_INSTITUICAO_ID).ToList();
                //var institution = databaseManager.INST_INSTITUICAO.Join(databaseManager.INST_INSTITUICAO_ENDERECOS,x => x.ID,y => y.INST_INSTITUICAO_ID, (x, y) => new { y }).Where(y => y.y.INST_INSTITUICAO_ID == INST_INSTITUICAO_ID).ToList();
                /*var institution = (from j1 in databaseManager.INST_INSTITUICAO
                                   join j2 in databaseManager.INST_INSTITUICAO_ENDERECOS on j1.ID equals j2.INST_INSTITUICAO_ID
                                   join j3 in databaseManager.INST_NSTITUICAO_CONTACTOS on j1.ID equals j3.INST_INSTITUICAO_ID
                                   where j1.ID==INST_INSTITUICAO_ID
                                   select new { j2.MORADA,j3.URL }).ToList();*/
               
                //var modules = databaseManager.INST_APLICACOES_MODULOS.Where(x => x.DATA_REMOCAO == null).Select(x => new {x.ID,x.NOME}).ToList();
                //var currency = databaseManager.GPAG_MOEDAS.Where(x => x.DATA_REMOCAO == null).Select(x => new {x.ID,x.CODIGO,x.SIGLA}).ToList();
                //var alectivo = databaseManager.GRL_ANO_LECTIVO.Where(x => x.DATA_REMOCAO == null && x.PADRAO_ACTIVO==true).Select(x => new { x.ID, x.VALOR, x.DESCRICAO }).ToList();

                INST_INSTITUICAO_SIGLA = configvalues[0].y.SIGLA;
                INST_INSTITUICAO_NOME = configvalues[0].y.NOME;
                //INST_INSTITUICAO_ENDERECO = institution[0].MORADA;
                //INST_INSTITUICAO_URL = institution[0].URL;
                INST_PER_TEMA_1 = configvalues[0].x.INST_PER_TEMA_1;
                INST_PER_TEMA_1_SIDEBAR = configvalues[0].x.INST_PER_TEMA_1_SIDEBAR;
                INST_PER_TEMA_2 = configvalues[0].x.INST_PER_TEMA_2;
                INST_PER_LOGOTIPO_WIDTH = configvalues[0].x.INST_PER_LOGOTIPO_WIDTH;
                /*
                INST_MDL_ADM_VLRID_MODULO_CAND = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_CAND;
                INST_MDL_ADM_VLRID_MODULO_PORTAL = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_PORTAL;
                INST_MDL_ADM_VLRID_MODULO_ADM = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_ADM;
                INST_MDL_ADM_VLRID_MODULO_GA = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_GA;
                INST_MDL_ADM_VLRID_MODULO_GPAG = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_GPAG;
                INST_MDL_ADM_VLRID_MODULO_GP = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_GP;
                INST_MDL_ADM_VLRID_MODULO_RPT = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_RPT;
                INST_MDL_ADM_VLRID_MODULO_HR = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_HR;
                INST_MDL_ADM_VLRID_MODULO_PD = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_PD;
                INST_MDL_ADM_VLRID_MODULO_BIB = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_BIB;
                INST_MDL_ADM_VLRID_MODULO_PWBI = configvalues[0].x.INST_MDL_ADM_VLRID_MODULO_PWBI;
                //-
                INST_MDL_ADM_MODULO_CAND = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_CAND).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_PORTAL = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_PORTAL).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_ADM = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_ADM).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_GA = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_GA).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_GPAG = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_GPAG).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_GP = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_GP).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_RPT = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_RPT).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_HR = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_HR).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_PD = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_PD).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_BIB = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_BIB).Select(x => x.NOME).SingleOrDefault();
                INST_MDL_ADM_MODULO_PWBI = modules.Where(x => x.ID == INST_MDL_ADM_VLRID_MODULO_PWBI).Select(x => x.NOME).SingleOrDefault();
                //-
                INST_MDL_ADM_VLRID_ESTADO_AP = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_AP;
                INST_MDL_ADM_VLRID_ESTADO_INSCRITO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_INSCRITO;
                INST_MDL_ADM_VLRID_ESTADO_ADMITIDO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_ADMITIDO;
                INST_MDL_ADM_VLRID_ESTADO_NAO_ADMITIDO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_NAO_ADMITIDO;
                INST_MDL_ADM_VLRID_ESTADO_ANULADO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_ANULADO;
                INST_MDL_ADM_VLRID_ESTADO_MATRICULADO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_MATRICULADO;
                INST_MDL_ADM_VLRID_ESTADO_NORMAL = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_NORMAL;
                INST_MDL_ADM_VLRID_ESTADO_EDICAO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_EDICAO;
                INST_MDL_ADM_VLRID_ESTADO_FINALIZADO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_FINALIZADO;
                INST_MDL_ADM_VLRID_ESTADO_PUBLICADO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_PUBLICADO;
                INST_MDL_ADM_VLRID_ESTADO_CONCLUIDO= configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_CONCLUIDO;
                INST_MDL_ADM_VLRID_ESTADO_FREQUENTAR= configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_FREQUENTAR;
                INST_MDL_ADM_VLRID_ESTADO_MUDANCA_CURSO= configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_MUDANCA_CURSO;
                INST_MDL_ADM_VLRID_ESTADO_AN_MATRICULA= configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_AN_MATRICULA;
                INST_MDL_ADM_VLRID_ESTADO_SU_MATRICULA= configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_SU_MATRICULA;
                INST_MDL_ADM_VLRID_ESTADO_AV = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_AV;
                INST_MDL_ADM_VLRID_ESTADO_NV = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_NV;
                INST_MDL_ADM_VLRID_ESTADO_CRIACAO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_CRIACAO;
                INST_MDL_ADM_VLRID_ESTADO_VALIDADA = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_VALIDADA;
                INST_MDL_ADM_VLRID_ESTADO_APU = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_APU;
                INST_MDL_ADM_VLRID_ESTADO_EXP = configvalues[0].x.INST_MDL_ADM_VLRID_ESTADO_EXP;
                INST_MDL_ADM_VLRID_RESIDENCIA_PROPIA = configvalues[0].x.INST_MDL_ADM_VLRID_RESIDENCIA_PROPIA;
                INST_MDL_ADM_VLRID_PROFISSAO_ESTUDANTE = configvalues[0].x.INST_MDL_ADM_VLRID_PROFISSAO_ESTUDANTE;
                INST_MDL_ADM_VLRID_REGIME_PROFISSIONAL = configvalues[0].x.INST_MDL_ADM_VLRID_REGIME_PROFISSIONAL;
                INST_MDL_ADM_VLRID_REGIME_TRANSFERENCIA = configvalues[0].x.INST_MDL_ADM_VLRID_REGIME_TRANSFERENCIA;
                INST_MDL_ADM_VLRID_UTILIZADOR_CAND_GERAL = configvalues[0].x.INST_MDL_ADM_VLRID_UTILIZADOR_CAND_GERAL;
                INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_TMP = configvalues[0].x.INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_TMP;
                INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_ALUNO = configvalues[0].x.INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_ALUNO;
                INST_MDL_ADM_VLRID_ESTUDO_PROCEDENCIA_ESTRANGEIRO = configvalues[0].x.INST_MDL_ADM_VLRID_ESTUDO_PROCEDENCIA_ESTRANGEIRO;
                INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY = configvalues[0].x.INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY;
                INST_MDL_ADM_VLRID_ARQUIVO_FOTOGRAFIA = configvalues[0].x.INST_MDL_ADM_VLRID_ARQUIVO_FOTOGRAFIA;
                INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO = configvalues[0].x.INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO;
                INST_MDL_ADM_VLRID_TIPODOC_BI = configvalues[0].x.INST_MDL_ADM_VLRID_TIPODOC_BI;
                INST_MDL_ADM_VLRID_PAUTAS_OBSERVACOES_FALTOU = configvalues[0].x.INST_MDL_ADM_VLRID_PAUTAS_OBSERVACOES_FALTOU;
                INST_MDL_GA_CARACT_ESP_PARAM = configvalues[0].x.INST_MDL_GA_CARACT_ESP_PARAM;
                INST_MDL_GA_PLANOS_ESTUDO_ANOS_ACADEMICOS = configvalues[0].x.INST_MDL_GA_PLANOS_ESTUDO_ANOS_ACADEMICOS;
                INST_MDL_GA_PLANO_OCORRENCIA_BASE_FORMULA = configvalues[0].x.INST_MDL_GA_PLANO_OCORRENCIA_BASE_FORMULA;
                INST_MDL_GPAG_MOEDA_PADRAO = configvalues[0].x.INST_MDL_GPAG_MOEDA_PADRAO;
                */
                //-
                //INST_MDL_GPAG_MOEDA_PADRAO_CODIGO = currency.Where(x => x.ID == INST_MDL_GPAG_MOEDA_PADRAO).Select(x => x.CODIGO).SingleOrDefault();
                //INST_MDL_GPAG_MOEDA_PADRAO_SIGLA= currency.Where(x => x.ID == INST_MDL_GPAG_MOEDA_PADRAO).Select(x => x.SIGLA).SingleOrDefault();
                //-
                //INST_MDL_GPAG_NUMERO_COPIAS_RECIBO = configvalues[0].x.INST_MDL_GPAG_NUMERO_COPIAS_RECIBO;
                //INST_MDL_GPAG_EMOL_DATA_LIMITE = configvalues[0].x.INST_MDL_GPAG_EMOL_DATA_LIMITE;
                INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS = configvalues[0].x.INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS;
                INST_MDL_GPAG_NOTA_DECIMAL = configvalues[0].x.INST_MDL_GPAG_NOTA_DECIMAL;
                //INST_MDL_GP_BI_MAXLENGTH = configvalues[0].x.INST_MDL_GP_BI_MAXLENGTH;
                /* = configvalues[0].x.NET_LDAP_BASE;
                NET_LDAP_HOSTNAME = configvalues[0].x.NET_LDAP_HOSTNAME;
                NET_ENDERECO_IP_INTERNO = configvalues[0].x.NET_ENDERECO_IP_INTERNO;
                NET_ENDERECO_IP_EXTERNO = configvalues[0].x.NET_ENDERECO_IP_EXTERNO;
                */
                NET_STMP_HOST = configvalues[0].x.NET_STMP_HOST;
                NET_STMP_PORT = configvalues[0].x.NET_STMP_PORT;
                NET_SMTP_USERNAME = configvalues[0].x.NET_SMTP_USERNAME;
                NET_SMTP_SENHA = configvalues[0].x.NET_SMTP_SENHA;
                NET_STMP_FROM = configvalues[0].x.NET_SMTP_FROM;

                SEC_SENHA_TENT_BLOQUEIO = configvalues[0].x.SEC_SENHA_TENT_BLOQUEIO;
                SEC_SENHA_TENT_BLOQUEIO_TEMPO = configvalues[0].x.SEC_SENHA_TENT_BLOQUEIO_TEMPO;
                SEC_SENHA_RECU_LIMITE_EMAIL = configvalues[0].x.SEC_SENHA_RECU_LIMITE_EMAIL;
                SEC_SESSAO_TIMEOUT_TEMPO = configvalues[0].x.SEC_SESSAO_TIMEOUT_TEMPO;
                /*API_CURRENCY_TOKEN = configvalues[0].x.API_CURRENCY_TOKEN;
                API_CURRENCY_BASE = configvalues[0].x.API_CURRENCY_BASE;
                API_SMS_TOKEN = configvalues[0].x.API_SMS_TOKEN;
                API_SMS_BASE = configvalues[0].x.API_SMS_BASE;
                API_SMS_ACTIVO = configvalues[0].x.API_SMS_ACTIVO;
                API_SMS_SENDER_ID = configvalues[0].x.API_SMS_SENDER_ID;*/
                //-
                //GRL_A_LECTIVO = alectivo.Any() ? alectivo[0].VALOR : DateTime.Now.Year;

            }
        }
    }
}