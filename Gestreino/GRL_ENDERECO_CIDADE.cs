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
    
    public partial class GRL_ENDERECO_CIDADE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GRL_ENDERECO_CIDADE()
        {
            this.INST_APLICACAO_ENDERECOS = new HashSet<INST_APLICACAO_ENDERECOS>();
            this.PES_ENDERECOS = new HashSet<PES_ENDERECOS>();
            this.PES_NATURALIDADE = new HashSet<PES_NATURALIDADE>();
            this.GRL_ENDERECO_MUN_DISTR = new HashSet<GRL_ENDERECO_MUN_DISTR>();
            this.PES_IDENTIFICACAO_LOCAL_EM = new HashSet<PES_IDENTIFICACAO_LOCAL_EM>();
            this.PES_PESSOAS_FAM_ENDERECOS = new HashSet<PES_PESSOAS_FAM_ENDERECOS>();
        }
    
        public int ID { get; set; }
        public int ENDERECO_PAIS_ID { get; set; }
        public string SIGLA { get; set; }
        public string NOME { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INST_APLICACAO_ENDERECOS> INST_APLICACAO_ENDERECOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_ENDERECOS> PES_ENDERECOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_NATURALIDADE> PES_NATURALIDADE { get; set; }
        public virtual GRL_ENDERECO_PAIS GRL_ENDERECO_PAIS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRL_ENDERECO_MUN_DISTR> GRL_ENDERECO_MUN_DISTR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_IDENTIFICACAO_LOCAL_EM> PES_IDENTIFICACAO_LOCAL_EM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_FAM_ENDERECOS> PES_PESSOAS_FAM_ENDERECOS { get; set; }
    }
}
