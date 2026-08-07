// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagedList.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging/PagedList.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging/PagedList.cs
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalCount">
        /// The total count.
        /// </param>
        public PagedList(IEnumerable<T> source, int index, int pageSize, int? totalCount = null)
            : this(source.AsQueryable(), index, pageSize, totalCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="pageNum">
        /// The page num.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="totalCount">
        /// The total count.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public PagedList(IQueryable<T> source, int pageNum, int pageSize, int? totalCount = null)
        {
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNum), "Value can not be below 0.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Value can not be less than 1.");
            }

            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            var realTotalCount = source.Count();

            this.PageSize = pageSize;
            this.PageIndex = pageNum - 1;
            this.TotalItemCount = totalCount ?? realTotalCount;
            this.PageCount = this.TotalItemCount > 0
                                 ? (int)Math.Ceiling(this.TotalItemCount / (double)this.PageSize)
                                 : 0;

            this.HasPreviousPage = this.PageIndex > 0;
            this.HasNextPage = this.PageIndex < (this.PageCount - 1);
            this.IsFirstPage = this.PageIndex <= 0;
            this.IsLastPage = this.PageIndex >= (this.PageCount - 1);

            this.ItemStart = this.PageIndex * this.PageSize + 1;
            this.ItemEnd = Math.Min(this.PageIndex * this.PageSize + this.PageSize, this.TotalItemCount);

            if (this.TotalItemCount <= 0)
            {
                return;
            }

            var realTotalPages = (int)Math.Ceiling(realTotalCount / (double)this.PageSize);

            if (realTotalCount < this.TotalItemCount && realTotalPages <= this.PageIndex)
            {
                this.AddRange(source.Skip((realTotalPages - 1) * this.PageSize).Take(this.PageSize));
            }
            else
            {
                this.AddRange(source.Skip(this.PageIndex * this.PageSize).Take(this.PageSize));
            }
        }

        #region IPagedList Members

        /// <summary>
        ///     Gets the page count.
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        ///     Gets the total item count.
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        ///     Gets the page index.
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        ///     The page number.
        /// </summary>
        public int PageNumber => this.PageIndex + 1;

        /// <summary>
        ///     Gets the page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        ///     Gets a value indicating whether has previous page.
        /// </summary>
        public bool HasPreviousPage { get; }

        /// <summary>
        ///     Gets a value indicating whether has next page.
        /// </summary>
        public bool HasNextPage { get; }

        /// <summary>
        ///     Gets a value indicating whether is first page.
        /// </summary>
        public bool IsFirstPage { get; }

        /// <summary>
        ///     Gets a value indicating whether is last page.
        /// </summary>
        public bool IsLastPage { get; }

        /// <summary>
        ///     Gets the item start.
        /// </summary>
        public int ItemStart { get; }

        /// <summary>
        ///     Gets the item end.
        /// </summary>
        public int ItemEnd { get; }

        /// <summary>
        ///     The get paging meta data.
        /// </summary>
        /// <returns>
        ///     The <see cref="PagingMetaData" />.
        /// </returns>
        public PagingMetaData GetPagingMetaData()
        {
            return new PagingMetaData
                       {
                           HasNextPage = this.HasNextPage, 
                           HasPreviousPage = this.HasPreviousPage, 
                           IsFirstPage = this.IsFirstPage, 
                           IsLastPage = this.IsLastPage, 
                           ItemEnd = this.ItemEnd, 
                           ItemStart = this.ItemStart, 
                           PageCount = this.PageCount, 
                           PageIndex = this.PageIndex, 
                           PageNumber = this.PageNumber, 
                           PageSize = this.PageSize, 
                           TotalItemCount = this.TotalItemCount
                       };
        }

        #endregion
    }

    /// <summary>
    ///     The paging meta data.
    /// </summary>
    public class PagingMetaData
    {
        /// <summary>
        ///     Gets or sets the page count.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        ///     Gets or sets the total item count.
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        ///     Gets or sets the page index.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     Gets or sets the page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        ///     Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether has previous page.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether has next page.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is first page.
        /// </summary>
        public bool IsFirstPage { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether is last page.
        /// </summary>
        public bool IsLastPage { get; set; }

        /// <summary>
        ///     Gets or sets the item start.
        /// </summary>
        public int ItemStart { get; set; }

        /// <summary>
        ///     Gets or sets the item end.
        /// </summary>
        public int ItemEnd { get; set; }
    }
}