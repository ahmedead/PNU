// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUoW.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   http://codereview.stackexchange.com/questions/19037/entity-framework-generic-repository-pattern
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Data
{
    using System;

    /// <summary>
    ///     http://codereview.stackexchange.com/questions/19037/entity-framework-generic-repository-pattern
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="userId">
        /// The user identifier.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int Save(string userId = null);
    }
}