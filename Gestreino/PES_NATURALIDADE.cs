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
    
    public partial class PES_NATURALIDADE
    {
        public int ID { get; set; }
        public int PES_PESSOAS_ID { get; set; }
        public int GRL_ENDERECO_PAIS_ID { get; set; }
        public int GRL_ENDERECO_CIDADE_ID { get; set; }
        public Nullable<int> GRL_ENDERECO_MUN_DISTR_ID { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual GRL_ENDERECO_CIDADE GRL_ENDERECO_CIDADE { get; set; }
        public virtual GRL_ENDERECO_MUN_DISTR GRL_ENDERECO_MUN_DISTR { get; set; }
        public virtual GRL_ENDERECO_PAIS GRL_ENDERECO_PAIS { get; set; }
        public virtual PES_PESSOAS PES_PESSOAS { get; set; }
    }
}
