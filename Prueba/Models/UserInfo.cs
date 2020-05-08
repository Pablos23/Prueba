using Prueba.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prueba.Models
{
    public class UserInfo : TableBase
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string ProfileImage { get; set; }
    }
}
