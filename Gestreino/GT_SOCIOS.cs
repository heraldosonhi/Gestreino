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
    
    public partial class GT_SOCIOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_SOCIOS()
        {
            this.GT_TreinosPessoa = new HashSet<GT_TreinosPessoa>();
            this.GT_RespAnsiedadeDepressao = new HashSet<GT_RespAnsiedadeDepressao>();
            this.GT_RespAutoConceito = new HashSet<GT_RespAutoConceito>();
            this.GT_RespRisco = new HashSet<GT_RespRisco>();
            this.GT_RespProblemasSaude = new HashSet<GT_RespProblemasSaude>();
            this.GT_RespFlexiTeste = new HashSet<GT_RespFlexiTeste>();
            this.GT_RespComposicao = new HashSet<GT_RespComposicao>();
            this.GT_RespAptidaoCardio = new HashSet<GT_RespAptidaoCardio>();
        }
    
        public int ID { get; set; }
        public int PES_PESSOAS_ID { get; set; }
        public int NUMERO { get; set; }
    
        public virtual PES_PESSOAS PES_PESSOAS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_TreinosPessoa> GT_TreinosPessoa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespAnsiedadeDepressao> GT_RespAnsiedadeDepressao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespAutoConceito> GT_RespAutoConceito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespRisco> GT_RespRisco { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespProblemasSaude> GT_RespProblemasSaude { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespFlexiTeste> GT_RespFlexiTeste { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespComposicao> GT_RespComposicao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespAptidaoCardio> GT_RespAptidaoCardio { get; set; }
    }
}
