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
    
    public partial class GT_TipoTesteComposicao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_TipoTesteComposicao()
        {
            this.GT_RespComposicao = new HashSet<GT_RespComposicao>();
        }
    
        public int ID { get; set; }
        public int GT_TipoMetodoComposicao_ID { get; set; }
        public string DESCRICAO { get; set; }
    
        public virtual GT_TipoMetodoComposicao GT_TipoMetodoComposicao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespComposicao> GT_RespComposicao { get; set; }
    }
}