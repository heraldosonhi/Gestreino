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
    
    public partial class PES_PESSOAS_FAM_CONTACTOS
    {
        public int ID { get; set; }
        public int PES_PESSOAS_FAM_ID { get; set; }
        public decimal TELEFONE { get; set; }
        public Nullable<decimal> TELEFONE_ALTERNATIVO { get; set; }
        public Nullable<decimal> FAX { get; set; }
        public string EMAIL { get; set; }
        public string URL { get; set; }
    
        public virtual PES_PESSOAS_FAM PES_PESSOAS_FAM { get; set; }
    }
}
