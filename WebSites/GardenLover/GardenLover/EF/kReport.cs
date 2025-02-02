//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenLover.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class kReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kReport()
        {
            this.kCompes = new HashSet<kCompe>();
            this.kJobs = new HashSet<kJob>();
            this.kJobDetails = new HashSet<kJobDetail>();
            this.kNextPlans = new HashSet<kNextPlan>();
            this.kNextPlanDetails = new HashSet<kNextPlanDetail>();
            this.kReport_History = new HashSet<kReport_History>();
        }
    
        public long Id { get; set; }
        public System.DateTime Time { get; set; }
        public long CorporationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        public System.Guid UserId { get; set; }
        public long Year { get; set; }
        public long Month { get; set; }
        public long StepId { get; set; }
        public long StateId { get; set; }
        public long SendToRelationTypeId { get; set; }
        public string SendToRelationValue { get; set; }
        public Nullable<decimal> TotalMarkJob { get; set; }
        public Nullable<decimal> TotalMarkCompe { get; set; }
        public Nullable<decimal> FinalMarkJob { get; set; }
        public Nullable<decimal> FinalMarkCompe { get; set; }
        public Nullable<decimal> FinalMarkKpi { get; set; }
        public string Comment { get; set; }
        public string ManagerComment { get; set; }
        public string HRcomment { get; set; }
        public string BODcomment { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kCompe> kCompes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kJob> kJobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kJobDetail> kJobDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kNextPlan> kNextPlans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kNextPlanDetail> kNextPlanDetails { get; set; }
        public virtual kRelationTypeForAll kRelationTypeForAll { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kReport_History> kReport_History { get; set; }
        public virtual kState kState { get; set; }
        public virtual kStep kStep { get; set; }
    }
}
