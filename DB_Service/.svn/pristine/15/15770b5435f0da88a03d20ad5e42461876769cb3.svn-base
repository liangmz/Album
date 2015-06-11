using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DB_Service
{
    public class Service<T> where T : class
    {
        public ablumEntities AE = new ablumEntities();

        public void Insert(T entity)
        {
            AE.Set<T>().Add(entity);
            AE.SaveChanges();
        }

        public void Delete(T entity)
        {
            AE.Set<T>().Remove(entity);
            AE.SaveChanges();
        }

        public IQueryable<T> Table()
        {
            return AE.Set<T>();
        }

        /// <summary>
        /// 计算页数
        /// </summary>
        /// <param name="Count">总数</param>
        /// <param name="PageCount">每页数量</param>
        /// <returns></returns>
        public int GetPageCount(int Count, int PageCount)
        {
            return Count / PageCount + (Count % PageCount != 0 ? 1 : 0);
        }
    }
}
