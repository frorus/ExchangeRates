using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRates.Filters
{
    //Класс для пагинации страниц
    public class PaginationFilter<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginationFilter(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public static PaginationFilter<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginationFilter<T>(items, count, pageIndex, pageSize);
        }
    }
}