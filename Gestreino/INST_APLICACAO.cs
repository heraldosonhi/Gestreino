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
    
    public partial class INST_APLICACAO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public INST_APLICACAO()
        {
            this.INST_APLICACAO_ARQUIVOS = new HashSet<INST_APLICACAO_ARQUIVOS>();
            this.INST_APLICACAO_ENDERECOS = new HashSet<INST_APLICACAO_ENDERECOS>();
            this.INST_APLICACAO_CONTACTOS = new HashSet<INST_APLICACAO_CONTACTOS>();
            this.GRL_DEFINICOES = new HashSet<GRL_DEFINICOES>();
        }
    
        public int ID { get; set; }
        public string SIGLA { get; set; }
        public string NOME { get; set; }
        public string NIF { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INST_APLICACAO_ARQUIVOS> INST_APLICACAO_ARQUIVOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INST_APLICACAO_ENDERECOS> INST_APLICACAO_ENDERECOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INST_APLICACAO_CONTACTOS> INST_APLICACAO_CONTACTOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GRL_DEFINICOES> GRL_DEFINICOES { get; set; }
    }
}
