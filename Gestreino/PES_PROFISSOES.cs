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
    
    public partial class PES_PROFISSOES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PES_PROFISSOES()
        {
            this.PES_PESSOAS_PROFISSOES = new HashSet<PES_PESSOAS_PROFISSOES>();
            this.PES_PESSOAS_FAM = new HashSet<PES_PESSOAS_FAM>();
        }
    
        public int ID { get; set; }
        public string NOME { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_PROFISSOES> PES_PESSOAS_PROFISSOES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_FAM> PES_PESSOAS_FAM { get; set; }
    }
}
