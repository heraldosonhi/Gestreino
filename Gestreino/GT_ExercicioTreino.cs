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
    
    public partial class GT_ExercicioTreino
    {
        public int ID { get; set; }
        public int GT_Treino_ID { get; set; }
        public int GT_Exercicio_ID { get; set; }
        public int GT_Series_ID { get; set; }
        public int GT_Repeticoes_ID { get; set; }
        public int GT_TempoDescanso_ID { get; set; }
        public int GT_Carga_ID { get; set; }
        public int GT_CoeficienteRepeticao_ID { get; set; }
        public Nullable<decimal> CARGA_USADA { get; set; }
        public Nullable<decimal> ONERM { get; set; }
        public int ORDEM { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual GT_Carga GT_Carga { get; set; }
        public virtual GT_CoeficienteRepeticao GT_CoeficienteRepeticao { get; set; }
        public virtual GT_Exercicio GT_Exercicio { get; set; }
        public virtual GT_Repeticoes GT_Repeticoes { get; set; }
        public virtual GT_Series GT_Series { get; set; }
        public virtual GT_TempoDescanso GT_TempoDescanso { get; set; }
        public virtual GT_Treino GT_Treino { get; set; }
    }
}
