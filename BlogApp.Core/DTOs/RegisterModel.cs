using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs
{
    public class RegisterModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
