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
    
    public partial class PES_PESSOAS_FAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PES_PESSOAS_FAM()
        {
            this.PES_PESSOAS_FAM_CONTACTOS = new HashSet<PES_PESSOAS_FAM_CONTACTOS>();
            this.PES_PESSOAS_FAM_ENDERECOS = new HashSet<PES_PESSOAS_FAM_ENDERECOS>();
        }
    
        public int ID { get; set; }
        public int PES_PESSOAS_ID { get; set; }
        public int PES_FAMILIARES_GRUPOS_ID { get; set; }
        public string NOME { get; set; }
        public Nullable<int> PES_PROFISSOES_ID { get; set; }
        public bool ISENTO { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual PES_FAMILIARES_GRUPOS PES_FAMILIARES_GRUPOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_FAM_CONTACTOS> PES_PESSOAS_FAM_CONTACTOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_FAM_ENDERECOS> PES_PESSOAS_FAM_ENDERECOS { get; set; }
        public virtual PES_PESSOAS PES_PESSOAS { get; set; }
        public virtual PES_PROFISSOES PES_PROFISSOES { get; set; }
    }
}
