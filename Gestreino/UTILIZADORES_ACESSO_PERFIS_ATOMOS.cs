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
    
    public partial class UTILIZADORES_ACESSO_PERFIS_ATOMOS
    {
        public int ID { get; set; }
        public int UTILIZADORES_ACESSO_ATOMOS_ID { get; set; }
        public int UTILIZADORES_ACESSO_PERFIS_ID { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual UTILIZADORES_ACESSO_ATOMOS UTILIZADORES_ACESSO_ATOMOS { get; set; }
        public virtual UTILIZADORES_ACESSO_PERFIS UTILIZADORES_ACESSO_PERFIS { get; set; }
    }
}
