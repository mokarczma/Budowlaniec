//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuilderApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CompanyLocality
    {
        public int ID { get; set; }
        public int company_id { get; set; }
        public int locality_id { get; set; }
        public bool isDelete { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Locality Locality { get; set; }
    }
}
