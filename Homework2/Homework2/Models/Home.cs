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
    
    public partial class Home
    {
        public int userID { get; set; }
        public string home1 { get; set; }
    
        public virtual User User { get; set; }
    }
}
