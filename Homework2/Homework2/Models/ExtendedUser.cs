using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework2.Models
{
    public class ExtendedUser : User
    {
        public User user { get; set; }
        public List<User> userList { get; set; }
        public List<Role> roleList { get; set; }
        public List<User> userTableList { get; set; }
    }
}