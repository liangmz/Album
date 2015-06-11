﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Service.Service
{
    public class a_evaluationService : Service<a_evaluation>
    {
        public IQueryable<a_evaluation> List(int Page, out int PageCount, int Count)
        {
            IQueryable<a_evaluation> T = Table();
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderByDescending(A => A.e_time).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public IQueryable<a_evaluation> PicList(int PID, int Page, out int PageCount, int Count)
        {
            IQueryable<a_evaluation> T = Table().Where(A => A.e_upid == PID);
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderByDescending(A => A.e_time).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public bool Delete(int ID)
        {
            try
            {
                a_evaluation AU = AE.a_evaluation.First(A => A.e_id == ID);
                AE.a_evaluation.Remove(AU);
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
