using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBuddy.Api.Helpers
{
    public class PagedList<T> : List<T>
    {
        // we are also going to pass these below properties in our response header of http
        public int TotalCount { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalPages { get; }

        public PagedList(List<T> items, 
            int count, 
            int pageNumber,
            int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count/ (double)pageSize);

            //add items in the instance of PagedList
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
