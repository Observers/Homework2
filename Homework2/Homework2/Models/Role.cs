//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Homework2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public int roleID { get; set; }
        public string role { get; set; }
        public string description { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string createUser { get; set; }
        public Nullable<System.DateTime> modifyDate { get; set; }
        public string modifyUser { get; set; }
        public List<Role> ListA { get; set; }
        public List<Role> ListB { get; set; }
    }
}