using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework2.Models
{
    public class ExtendedRole : Role
    {
        public List<Role> rolesList { get; set; }
        public List<Role> rolesTableList { get; set; }
    }
}