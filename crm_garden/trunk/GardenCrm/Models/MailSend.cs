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
    
    public partial class MailSend
    {
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public string MailTitle { get; set; }
        public string ContentMail { get; set; }
        public string CcMailTo { get; set; }
        public System.DateTime DateCreate { get; set; }
        public Nullable<System.DateTime> DateSend { get; set; }
        public int NumberSend { get; set; }
        public bool Sended { get; set; }
    }
}
