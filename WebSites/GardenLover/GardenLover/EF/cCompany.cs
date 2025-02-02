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
    
    public partial class cCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cCompany()
        {
            this.cBranches = new HashSet<cBranch>();
            this.cDepartments = new HashSet<cDepartment>();
            this.cDepartmentForAlls = new HashSet<cDepartmentForAll>();
            this.cGrades = new HashSet<cGrade>();
            this.cGradeForAlls = new HashSet<cGradeForAll>();
            this.cRanks = new HashSet<cRank>();
            this.cRankForAlls = new HashSet<cRankForAll>();
            this.cUserInfoes = new HashSet<cUserInfo>();
        }
    
        public long Id { get; set; }
        public Nullable<long> ParentId { get; set; }
        public long CorporationId { get; set; }
        public string CompanyName { get; set; }
        public Nullable<long> Seq { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cBranch> cBranches { get; set; }
        public virtual cCorporation cCorporation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cDepartment> cDepartments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cDepartmentForAll> cDepartmentForAlls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cGrade> cGrades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cGradeForAll> cGradeForAlls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cRank> cRanks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cRankForAll> cRankForAlls { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cUserInfo> cUserInfoes { get; set; }
    }
}
