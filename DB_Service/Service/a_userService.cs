using DB_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Service.Service
{
    public class a_userService : Service<a_user>
    {
        public IQueryable<a_user> List(int Page, out int PageCount, int Count)
        {
            IQueryable<a_user> T = Table();
            PageCount = GetPageCount(T.Count(),Count);
            return T.OrderBy(A => A.u_id).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public a_user SelectUser(int ID)
        {
            a_user AU = AE.a_user.First(A => A.u_id == ID);
            return AU;
        }
        public bool Delete(int ID)
        {
            try
            {
                a_user AU = AE.a_user.First(A => A.u_id == ID);
                AE.a_user.Remove(AU);
                if (AE.SaveChanges() > 0)
                {
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }
    }
}
