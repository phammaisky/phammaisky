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
    
    public partial class cRank
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cRank()
        {
            this.cUserInfoes = new HashSet<cUserInfo>();
        }
    
        public long Id { get; set; }
        public long CorporationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        public long RankForAllId { get; set; }
        public Nullable<long> Seq { get; set; }
        public decimal xKpiJob { get; set; }
        public decimal xKpiCompe { get; set; }
        public bool CanBeManager { get; set; }
    
        public virtual cBranch cBranch { get; set; }
        public virtual cCompany cCompany { get; set; }
        public virtual cCorporation cCorporation { get; set; }
        public virtual cRankForAll cRankForAll { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cUserInfo> cUserInfoes { get; set; }
    }
}