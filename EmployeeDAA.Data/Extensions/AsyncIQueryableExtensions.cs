using EmployeeDAA.Core;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Extensions
{
    public static class AsyncIQueryableExtensions
    {
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize, bool getOnlyTotalCount = false)
        {
            if (source == null)
            {
                return new PagedList<T>(new List<T>(), pageIndex, pageSize);
            }

            //min allowed page size is 1
            pageSize = Math.Max(pageSize, 1);

            int count = await source.CountAsync();

            List<T> data = new();

            if (!getOnlyTotalCount)
            {
                data.AddRange(await source.Skip(pageIndex).Take(pageSize).ToListAsync());
            }
            else
            {
                data.AddRange(await source.Skip(pageIndex).Take(count == 0 ? 1 : count).ToListAsync());
            }

            return new PagedList<T>(data, pageIndex, pageSize, count);
        }
    }
}
