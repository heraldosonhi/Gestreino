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
    
    public partial class GT_Exercicio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GT_Exercicio()
        {
            this.GT_Exercicio_ARQUIVOS = new HashSet<GT_Exercicio_ARQUIVOS>();
            this.GT_ExercicioTreino = new HashSet<GT_ExercicioTreino>();
            this.GT_ExercicioTreinoCardio = new HashSet<GT_ExercicioTreinoCardio>();
        }
    
        public int ID { get; set; }
        public int GT_TipoTreino_ID { get; set; }
        public string NOME { get; set; }
        public Nullable<int> ALONGAMENTO { get; set; }
        public Nullable<int> SEQUENCIA { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_Exercicio_ARQUIVOS> GT_Exercicio_ARQUIVOS { get; set; }
        public virtual GT_TipoTreino GT_TipoTreino { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_ExercicioTreino> GT_ExercicioTreino { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GT_ExercicioTreinoCardio> GT_ExercicioTreinoCardio { get; set; }
    }
}
