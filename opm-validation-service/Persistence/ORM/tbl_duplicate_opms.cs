//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace opm_validation_service.Persistence.ORM
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_duplicate_opms
    {
        public int id { get; set; }
        public decimal tdo_cp_id { get; set; }
        public string tdo_ean { get; set; }
        public bool tdo_is_opm_duplicate { get; set; }
    }
}
