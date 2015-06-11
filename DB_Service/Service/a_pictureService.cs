﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Service.Service
{
    public class a_pictureService : Service<a_picture>
    {
        public IQueryable<a_picture> List(int Page, out int PageCount, int Count)
        {
            IQueryable<a_picture> T = Table();
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderBy(A => A.p_id).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public a_picture GetById(int PID)
        {
            return AE.a_picture.First(A => A.p_id == PID);
        }
        public bool Delete(int ID)
        {
            try
            {
                a_picture AU = AE.a_picture.First(A => A.p_id == ID);
                AE.a_picture.Remove(AU);
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
