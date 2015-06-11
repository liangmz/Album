using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DB_Service;

namespace AlbumAdmin.Models.Admin
{
    public class ListTypeModel
    {
        public int Page { get; set; }
        public string type { get; set; }
    }

    public class ListModel<T,N,M>
    {
        public int Page { get; set; }
        public int PageCount { get; set; }
        public List<T> List { get; set; }

        public N model_f { get; set; }
        public M model_s { get; set; }

        public int reload { get; set; }
        public string action { get; set; }

        public int AID { get; set; }
        public int UID { get; set; }
        public int UPID { get; set; }
        public int PID { get; set; }
    }
}