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
    
    public partial class UTILIZADORES_ACESSO_PERFIS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UTILIZADORES_ACESSO_PERFIS()
        {
            this.UTILIZADORES_ACESSO_PERFIS_ATOMOS = new HashSet<UTILIZADORES_ACESSO_PERFIS_ATOMOS>();
            this.UTILIZADORES_ACESSO_UTIL_PERFIS = new HashSet<UTILIZADORES_ACESSO_UTIL_PERFIS>();
        }
    
        public int ID { get; set; }
        public string NOME { get; set; }
        public string DESCRICAO { get; set; }
        public Nullable<int> INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UTILIZADORES_ACESSO_PERFIS_ATOMOS> UTILIZADORES_ACESSO_PERFIS_ATOMOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UTILIZADORES_ACESSO_UTIL_PERFIS> UTILIZADORES_ACESSO_UTIL_PERFIS { get; set; }
    }
}