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
    
    public partial class PES_PESSOAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PES_PESSOAS()
        {
            this.GT_SOCIOS = new HashSet<GT_SOCIOS>();
            this.PES_CONTACTOS = new HashSet<PES_CONTACTOS>();
            this.PES_ENDERECOS = new HashSet<PES_ENDERECOS>();
            this.PES_IDENTIFICACAO = new HashSet<PES_IDENTIFICACAO>();
            this.PES_NACIONALIDADE = new HashSet<PES_NACIONALIDADE>();
            this.PES_NATURALIDADE = new HashSet<PES_NATURALIDADE>();
            this.PES_PESSOAS_PROFISSOES = new HashSet<PES_PESSOAS_PROFISSOES>();
            this.PES_PESSOAS_FAM = new HashSet<PES_PESSOAS_FAM>();
            this.PES_PESSOAS_CARACT_DEFICIENCIA = new HashSet<PES_PESSOAS_CARACT_DEFICIENCIA>();
            this.PES_PESSOAS_CARACT = new HashSet<PES_PESSOAS_CARACT>();
            this.PES_ARQUIVOS = new HashSet<PES_ARQUIVOS>();
        }
    
        public int ID { get; set; }
        public int UTILIZADORES_ID { get; set; }
        public string NOME_PROPIO { get; set; }
        public string APELIDO { get; set; }
        public string NOME { get; set; }
        public string SEXO { get; set; }
        public Nullable<System.DateTime> DATA_NASCIMENTO { get; set; }
        public Nullable<int> PES_ESTADO_CIVIL_ID { get; set; }
        public string NIF { get; set; }
        public string APRESENTACAO_PESSOAL { get; set; }
        public string FOTOGRAFIA { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_SOCIOS> GT_SOCIOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_CONTACTOS> PES_CONTACTOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_ENDERECOS> PES_ENDERECOS { get; set; }
        public virtual PES_ESTADO_CIVIL PES_ESTADO_CIVIL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_IDENTIFICACAO> PES_IDENTIFICACAO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_NACIONALIDADE> PES_NACIONALIDADE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_NATURALIDADE> PES_NATURALIDADE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_PROFISSOES> PES_PESSOAS_PROFISSOES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_FAM> PES_PESSOAS_FAM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_CARACT_DEFICIENCIA> PES_PESSOAS_CARACT_DEFICIENCIA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_PESSOAS_CARACT> PES_PESSOAS_CARACT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PES_ARQUIVOS> PES_ARQUIVOS { get; set; }
        public virtual UTILIZADORES UTILIZADORES { get; set; }
    }
}
