﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB_Service;

namespace DB_Service.Service
{
    public class a_tabService : Service<a_tag>
    {
        public IQueryable<a_tag> List(int Page, out int PageCount, int Count)
        {
            IQueryable<a_tag> T = Table();
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderByDescending(A => A.t_id).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public bool Delete(int ID)
        {
            try
            {
                a_tag AU = AE.a_tag.First(A => A.t_id == ID);
                AE.a_tag.Remove(AU);
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
