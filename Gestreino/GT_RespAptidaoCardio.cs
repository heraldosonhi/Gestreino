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
    
    public partial class GT_RespAptidaoCardio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_RespAptidaoCardio()
        {
            this.GT_RespAptidaoCardioYMCA = new HashSet<GT_RespAptidaoCardioYMCA>();
        }
    
        public int ID { get; set; }
        public int GT_SOCIOS_ID { get; set; }
        public int GT_TipoTesteCardio_ID { get; set; }
        public Nullable<decimal> CARGA { get; set; }
        public Nullable<decimal> TEMPO { get; set; }
        public Nullable<decimal> FC4M { get; set; }
        public Nullable<decimal> FC5M { get; set; }
        public Nullable<decimal> FC400M { get; set; }
        public Nullable<decimal> FC3M { get; set; }
        public Nullable<decimal> FC15M { get; set; }
        public Nullable<decimal> FCFIMTESTE { get; set; }
        public Nullable<decimal> VELOCIDADE { get; set; }
        public Nullable<decimal> VELOCIDADEMPH { get; set; }
        public Nullable<decimal> MEDIA { get; set; }
        public Nullable<decimal> VO2MAX { get; set; }
        public Nullable<decimal> V02METS { get; set; }
        public Nullable<decimal> CUSTO { get; set; }
        public Nullable<decimal> CUSTOCAL { get; set; }
        public string VO2DESEJAVEL { get; set; }
        public Nullable<int> CLASSIFICACAO { get; set; }
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
        public virtual GT_TipoTesteCardio GT_TipoTesteCardio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_RespAptidaoCardioYMCA> GT_RespAptidaoCardioYMCA { get; set; }
    }
}
