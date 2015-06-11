﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Service.Service
{
    public class a_upictureService : Service<a_upicture>
    {
        public IQueryable<a_upicture> List(int Page, out int PageCount, int Count)
        {
            IQueryable<a_upicture> T = Table();
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderByDescending(A => A.up_uploadTime).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public IQueryable<a_upicture> AblumUPicList(int AID, int Page, out int PageCount, int Count)
        {
            IQueryable<a_upicture> T = Table().Where(A => A.up_abid == AID);
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderByDescending(A => A.up_uploadTime).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public a_upicture GetById(int UPID)
        {
            return AE.a_upicture.First(A => A.up_id == UPID);
        }
        public IQueryable<a_upicture> ListPID(int PID, int Page, out int PageCount, int Count)
        {
            IQueryable<a_upicture> T = Table().Where(A=>A.up_pid == PID);
            PageCount = GetPageCount(T.Count(), Count);
            return T.OrderByDescending(A => A.up_uploadTime).Skip((Page - 1) * Count < 0 ? 0 : (Page - 1) * Count).Take(Count);
        }
        public bool Delete(int PID)
        {
            try
            {
                a_upicture UP = AE.a_upicture.First(A => A.up_id == PID);
                AE.a_upicture.Remove(UP);
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
