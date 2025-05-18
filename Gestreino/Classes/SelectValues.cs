namespace Gestreino.Classes
{
    public class SelectValues
    {
        public enum Status
        {
            Activo=1,
            Inactivo=0
        }
        public enum InternoExterno
        {
            Interno,
            Externo
        }
        public enum GrauDef
        {
            Total=1,
            Parcial=2
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
        public enum ValorMonetario
        {
            Percentagem = 1,
            Valor = 2,
        }
        public enum ExportFormat
        {
            PDF = 1,
            Excel = 2
        }
        public enum ExportFormatPDFLayout
        {
            Retrato = 1,
            Horizontal = 2,
        }
        public enum Duration
        {
            Semanas = 1,
            Aulas = 2
        }
    }
}