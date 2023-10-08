using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class AddToRoleModel
    {
        public string SelectedEmail { get; set; }
        public string SelectedRole { get; set; }
        public List<string> Emails { get; set; }
        public List<string> Roles { get; set; }

        public AddToRoleModel()
        {
            Emails = new List<string>();
            Roles = new List<string>();
        }
    }
}