using System;
using System.Collections.Generic;
using System.Linq;

namespace Pos.Internals.Extensions
{
    /// <summary>
    /// Represent a class to implements Paging functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T> : List<T>
    {
        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = source.Count();
            this.TotalPages = (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);

            this.AddRange(source.Skip(this.PageIndex * this.PageSize).Take(this.PageSize));
        }

        /// <summary>
        /// Gets a value that indicate current page index (Starts by Zero).
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Gets a value that indicate each page size.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Gets a value that indicate count of all rows in data source.
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Gets a value that indicate count of pages in data source.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets a value that indicate that does previous page exists or not.
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return (this.PageIndex > 0);
            }
        }

        /// <summary>
        /// Gets a value that indicate that does next page exists or not.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return (this.PageIndex + 1 < this.TotalPages);
            }
        }
    }
}