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
    
    public partial class GT_TipoTesteCardio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_TipoTesteCardio()
        {
            this.GT_RespAptidaoCardio = new HashSet<GT_RespAptidaoCardio>();
        }
    
        public int ID { get; set; }
        public int GT_TipoMetodoCardio_ID { get; set; }
        public string DESCRICAO { get; set; }
    
        public virtual GT_TipoMetodoCardio GT_TipoMetodoCardio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespAptidaoCardio> GT_RespAptidaoCardio { get; set; }
    }
}