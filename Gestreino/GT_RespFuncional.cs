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
    using System.Collections.Generic;
    
    public partial class GT_RespFuncional
    {
        public int ID { get; set; }
        public int GT_SOCIOS_ID { get; set; }
        public Nullable<int> RESP_01 { get; set; }
        public Nullable<int> RESP_02 { get; set; }
        public Nullable<int> RESP_03 { get; set; }
        public Nullable<int> RESP_04 { get; set; }
        public Nullable<int> RESP_05 { get; set; }
        public Nullable<int> RESP_06 { get; set; }
        public Nullable<int> RESP_07 { get; set; }
        public string DESPORTO { get; set; }
        public string POSICAO { get; set; }
        public Nullable<int> MAO { get; set; }
        public Nullable<int> PERNA { get; set; }
        public Nullable<int> OLHO { get; set; }
        public Nullable<decimal> RESP_SUMMARY { get; set; }
        public string RESP_DESCRICAO { get; set; }
        public Nullable<int> PERCENTIL { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual GT_SOCIOS GT_SOCIOS { get; set; }
    }
}
