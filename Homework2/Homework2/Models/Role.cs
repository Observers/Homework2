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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            this.Users = new HashSet<User>();
            this.Menus = new HashSet<Menu>();
        }
    
        public int roleID { get; set; }
        public string roleName { get; set; }
        public string description { get; set; }
        public Nullable<bool> status { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string createUser { get; set; }
        public Nullable<System.DateTime> modifyDate { get; set; }
        public string modifyUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Menu> Menus { get; set; }

        public List<Role> rolesList { get; set; }
        public List<Role> rolesTableList { get; set; }
    }
}
