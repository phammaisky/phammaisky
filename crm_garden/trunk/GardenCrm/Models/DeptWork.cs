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
    
    public partial class DeptWork
    {
        public int Id { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<decimal> WorkHoursPerDay { get; set; }
        public Nullable<bool> Active { get; set; }
    }
}
