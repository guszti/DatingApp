using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class Grid<T> : List<T>
    {
        public int Page { get; set; }
        
        public int Limit { get; set; }
        
        public int Total { get; set; }
        
        public int TotalPages { get; set; }

        public Grid(IEnumerable<T> items, int total, int page, int limit)
        {
            this.Page = page;
            this.Limit = limit;
            this.Total = total;
            this.TotalPages = (int) Math.Ceiling(total / (double) limit);

            AddRange(items);
        }

        public static async Task<Grid<T>> CreateGridAsync(IQueryable<T> query, int page, int limit)
        {
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
            
            return new Grid<T>(items, total, page, limit);
        }
    }
}