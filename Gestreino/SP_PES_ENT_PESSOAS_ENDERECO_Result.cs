//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestreino
{
    using System;
    
    public partial class SP_PES_ENT_PESSOAS_ENDERECO_Result
    {
        public int ID { get; set; }
        public int PES_TIPO_ENDERECOS_ID { get; set; }
        public string TIPO_END_SIGLA { get; set; }
        public string TIPO_END_NOME { get; set; }
        public bool ENDERECO_PRINCIAL { get; set; }
        public Nullable<int> NUMERO { get; set; }
        public string RUA { get; set; }
        public string MORADA { get; set; }
        public int GRL_ENDERECO_PAIS_ID { get; set; }
        public int GRL_ENDERECO_CIDADE_ID { get; set; }
        public Nullable<int> GRL_ENDERECO_MUN_DISTR_ID { get; set; }
        public string PAIS_NOME { get; set; }
        public string CIDADE_NOME { get; set; }
        public string MUN_NOME { get; set; }
        public string DATA_INICIO { get; set; }
        public string DATA_FIM { get; set; }
        public string INSERCAO { get; set; }
        public string ACTUALIZACAO { get; set; }
        public string DATA_INSERCAO { get; set; }
        public string DATA_ACTUALIZACAO { get; set; }
    }
}