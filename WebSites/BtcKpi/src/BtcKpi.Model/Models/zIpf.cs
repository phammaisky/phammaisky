//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BtcKpi.Model
{
    using System;
	using System.ComponentModel.DataAnnotations;
    
    public partial class zIpf
    {
		[Key]
        public int HistoryID { get; set; }
        public int ID { get; set; }
        public int UserId { get; set; }
        public Nullable<byte> PersonType { get; set; }
        public Nullable<byte> ScheduleType { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> ScheduleID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<byte> DeleteFlg { get; set; }
        public Nullable<System.DateTime> Deleted { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public string ManagerComment { get; set; }
        public string SelfComment { get; set; }
    }
}