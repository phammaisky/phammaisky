//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenCrm.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupUser_Authorize
    {
        public int GroupUserId { get; set; }
        public int AuthorizeId { get; set; }
        public Nullable<int> Extend { get; set; }
    
        public virtual Function Function { get; set; }
        public virtual GroupUser GroupUser { get; set; }
    }
}
