using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumAdmin.Models.Admin
{
    public class LoginModel
    {
        public string loginName { get; set; }
        public string password { get; set; }
        public string vCode { get; set; }
        public string error { get; set; }
    }
}