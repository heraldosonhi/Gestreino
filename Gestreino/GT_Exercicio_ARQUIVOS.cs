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
    
    public partial class GT_Exercicio_ARQUIVOS
    {
        public int ID { get; set; }
        public int GT_Exercicio_ID { get; set; }
        public int ARQUIVOS_ID { get; set; }
        public string NOME { get; set; }
        public string DESCRICAO { get; set; }
        public bool ACTIVO { get; set; }
        public Nullable<System.DateTime> DATA_ACT { get; set; }
        public Nullable<System.DateTime> DATA_DESACT { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual GRL_ARQUIVOS GRL_ARQUIVOS { get; set; }
        public virtual GT_Exercicio GT_Exercicio { get; set; }
    }
}
