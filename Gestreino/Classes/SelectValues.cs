using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gestreino.Classes
{
    public class SelectValues
    {
        public enum OAuth2
        {
            Microsoft,
            Facebook,
            Twitter,
            Google
        }
        public enum Status
        {
            Activo,
            Inactivo
        }
        public enum Duration
        {
            Dia = 1,
            Semana = 2,
            Mês = 3,
            Ano = 4
        }
        public enum InternoExterno
        {
            Interno,
            Externo
        }
        public enum PisoEntrada
        {
            Sim,
            Não
        }
        public enum TipoValor
        {
            Numérico = 1,
            Texto = 2,
            Data = 3
        }
        public enum CopiaRecibos
        {
            Unica = 1,
            Duplicada = 2,
            Triplicada = 3,
            Quadruplicada = 4
        }
        public enum NotaDecimal
        {
            Virgula = 1,
            Ponto = 2
        }
        public enum Escolha
        {
            Sim = 1,
            Não = 0
        }

        public enum Sexo
        {
            Masculino = 1,
            Feminino = 0
        }
        public enum Ocupacao
        {
            Estudante = 1,
            Trabalhador = 2
        }
        public enum ValorMonetario
        {
            Percentagem = 1,
            Valor = 2,
        }
        public enum ModalidadeTipologia
        {
            Diária = 1,
            Mensal = 2,
        }
        public enum PautaEscalaTipo
        {
            Quantitativa = 1,
            Qualitativa = 2,
        }
        public enum ExportFormat
        {
            PDF = 1
          //  Excel = 2,
        }
        public enum ExportFormatPDFLayout
        {
            Retrato = 1,
            Horizontal = 2,
        }
        public enum TipoTransferencia
        {
            Interna = 1,
            Externa = 2
        }
        public enum TipoPedidoEscolha
        {
            Declaração = 1,
            Pedido = 2
        }
        public enum PagamentoCondicaoSuplemento
        {
            Acesso = 1,
            Global = 2,
            PagamentoAdicional = 3,
            SuplementosFREQ = 4,
            SuplementosEXCLF=5,
        }
        public enum GATipoInscricao
        {
            Anual = 1,
            UC = 2
        }

        public enum GAPautaEpocaTipoExame
        {
            Fazer = 1,
            //Fazer_Melhoria = 2,
            Melhoria = 3
        }
        public enum GAFacultyCategoria
        {
            Geral = 1,
            Leitura = 2
        }

        public enum GPAGMovementTipo
        {
            Débito = 1,
            Crédito = 2
        }
        public enum GPAGFinesAculumacao
        {
            Fixa = 1,
            Constante = 2,
            Progressiva = 3
        }
        public enum GAStatutes
        {
            Descontos = 1,
            Bolsa = 2
        }
        public enum GAPrecendiaTipo
        {
            Inscrição=1,
            //Aprovação = 2,
            //Frequência = 3,
        }
    }
}