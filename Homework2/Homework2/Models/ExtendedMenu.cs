using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework2.Models
{
    public class ExtendedMenu : Menu
    {
        public Menu menu { get; set; }
        public List<Menu> menuList { get; set; }
        public List<Menu> menuTableList { get; set; }
        public List<string> menuNoList { get; set; }
        public string subMenuNo { get; set; }
    }
}