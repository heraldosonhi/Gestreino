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
    
    public partial class GT_RespProblemasSaude
    {
        public int ID { get; set; }
        public int GT_SOCIOS_ID { get; set; }
        public Nullable<bool> radOsteoporose { get; set; }
        public Nullable<System.DateTime> dtOsteoporoseI { get; set; }
        public Nullable<System.DateTime> dtOsteoporoseF { get; set; }
        public string txtOsteoporose { get; set; }
        public Nullable<bool> radOsteoartose { get; set; }
        public Nullable<System.DateTime> dtOsteoartoseI { get; set; }
        public Nullable<System.DateTime> dtOsteoartoseF { get; set; }
        public string txtOsteoartose { get; set; }
        public Nullable<bool> radArticulares { get; set; }
        public Nullable<System.DateTime> dtArticularesI { get; set; }
        public Nullable<System.DateTime> dtArticularesF { get; set; }
        public string txtArticulares { get; set; }
        public Nullable<bool> radLesoes { get; set; }
        public Nullable<System.DateTime> dtLesoesI { get; set; }
        public Nullable<System.DateTime> dtLesoesF { get; set; }
        public string txtLesoes { get; set; }
        public Nullable<bool> radDor { get; set; }
        public Nullable<System.DateTime> dtDorI { get; set; }
        public Nullable<System.DateTime> dtDorF { get; set; }
        public string txtDor { get; set; }
        public string txtCausaDor { get; set; }
        public Nullable<bool> radEscoliose { get; set; }
        public Nullable<System.DateTime> dtEscolioseI { get; set; }
        public Nullable<System.DateTime> dtEscolioseF { get; set; }
        public Nullable<bool> radHiperlordose { get; set; }
        public Nullable<System.DateTime> dtHiperlordoseI { get; set; }
        public Nullable<System.DateTime> dtHiperlordoseF { get; set; }
        public Nullable<bool> radHipercifose { get; set; }
        public Nullable<System.DateTime> dtHipercifoseI { get; set; }
        public Nullable<System.DateTime> dtHipercifoseF { get; set; }
        public Nullable<bool> radJoelho { get; set; }
        public Nullable<System.DateTime> dtJoelhoI { get; set; }
        public Nullable<System.DateTime> dtJoelhoF { get; set; }
        public string txtJoelho { get; set; }
        public Nullable<bool> radOmbro { get; set; }
        public Nullable<System.DateTime> dtOmbroI { get; set; }
        public Nullable<System.DateTime> dtOmbroF { get; set; }
        public string txtOmbro { get; set; }
        public Nullable<bool> radPunho { get; set; }
        public Nullable<System.DateTime> dtPunhoI { get; set; }
        public Nullable<System.DateTime> dtPunhoF { get; set; }
        public string txtPunho { get; set; }
        public Nullable<bool> radTornozelo { get; set; }
        public Nullable<System.DateTime> dtTornozeloI { get; set; }
        public Nullable<System.DateTime> dtTornozeloF { get; set; }
        public string txtTornozelo { get; set; }
        public Nullable<bool> radOutraArtic { get; set; }
        public Nullable<System.DateTime> dtOutraArticI { get; set; }
        public Nullable<System.DateTime> dtOutraArticF { get; set; }
        public string txtOutraArtic1 { get; set; }
        public string txtOutraArtic2 { get; set; }
        public Nullable<bool> radParkinson { get; set; }
        public Nullable<System.DateTime> dtParkinsonI { get; set; }
        public Nullable<bool> radVisual { get; set; }
        public Nullable<System.DateTime> dtVisualI { get; set; }
        public Nullable<System.DateTime> dtVisualF { get; set; }
        public string txtVisual { get; set; }
        public Nullable<bool> radAuditivo { get; set; }
        public Nullable<System.DateTime> dtAuditivoI { get; set; }
        public Nullable<System.DateTime> dtAuditivoF { get; set; }
        public string txtAuditivo { get; set; }
        public Nullable<bool> radGastro { get; set; }
        public Nullable<System.DateTime> dtGastroI { get; set; }
        public Nullable<System.DateTime> dtGastroF { get; set; }
        public string txtGastro { get; set; }
        public Nullable<bool> radCirugia { get; set; }
        public Nullable<int> txtCirugiaIdade1 { get; set; }
        public string txtCirugiaOnde1 { get; set; }
        public string txtCirugiaCausa1 { get; set; }
        public string txtCirugiaRestricao1 { get; set; }
        public Nullable<int> txtCirugiaIdade2 { get; set; }
        public string txtCirugiaOnde2 { get; set; }
        public string txtCirugiaCausa2 { get; set; }
        public string txtCirugiaRestricao2 { get; set; }
        public Nullable<bool> radProbSaude { get; set; }
        public string txtProbSaude { get; set; }
        public Nullable<bool> radInactividade { get; set; }
        public string txtInactividade { get; set; }
        public int INSERIDO_POR { get; set; }
        public Nullable<int> ACTUALIZADO_POR { get; set; }
        public Nullable<int> REMOVIDO_POR { get; set; }
        public System.DateTime DATA_INSERCAO { get; set; }
        public Nullable<System.DateTime> DATA_ACTUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_REMOCAO { get; set; }
    
        public virtual GT_SOCIOS GT_SOCIOS { get; set; }
    }
}
