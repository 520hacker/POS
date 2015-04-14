using System.Linq;

namespace Pos.Internals.Extensions
{
    public static class PaginatedListExtensions
    {
        /// <summary>
        /// Returns a paginated list.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="q">A IQueryable instance to apply.</param>
        /// <param name="pageIndex">Page number that starts with zero.</param>
        /// <param name="pageSize">Size of each page.</param>
        /// <returns>Returns a paginated list.</returns>
        /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        /// <example>
        ///     Following example shows how use this extension method in ASP.NET MVC web application.
        ///     <code>
        ///     public ActionResult Customers(int? page, int? size)
        ///     {
        ///         var q = from p in customers
        ///                 select p;
        ///     
        ///         return View(q.ToPaginatedList(page.HasValue ? page.Value : 1, size.HasValue ? size.Value : 15));
        ///     }
        ///     </code>
        /// </example>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> q, int pageIndex, int pageSize)
        {
            return new PaginatedList<T>(q, pageIndex, pageSize);
        }

        /// <summary>
        /// Returns a paginated list. This function returns 15 rows from specific pageIndex.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="q">A IQueryable instance to apply.</param>
        /// <param name="pageIndex">Page number that starts with zero.</param>    
        /// <returns>Returns a paginated list.</returns>
        /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> q, int pageIndex)
        {
            return new PaginatedList<T>(q, pageIndex, 15);
        }

        /// <summary>
        /// Returns a paginated list. This function returns 15 rows from page one.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="q">A IQueryable instance to apply.</param>    
        /// <returns>Returns a paginated list.</returns>
        /// <remarks>This functionality may not work in SQL Compact 3.5</remarks>
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> q)
        {
            return new PaginatedList<T>(q, 1, 15);
        }
    }
}