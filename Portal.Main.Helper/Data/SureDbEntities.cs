// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SureDbEntities.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The sure db entities.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Data
{
    using System.Data.Entity;

    /// <summary>
    ///     The sure db entities.
    /// </summary>
    internal class SureDbEntities : DbContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SureDbEntities" /> class.
        /// </summary>
        public SureDbEntities()
            : base("name=SureCommonDbEntities")
        {
            // Database.SetInitializer<SureDbEntities>(null);
        }

        /// <summary>
        ///     Gets or sets the logs.
        /// </summary>
        public virtual DbSet<Log> Logs { get; set; }

        /// <summary>
        ///     Gets or sets the system settings.
        /// </summary>
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }

        ///// <summary>
        ///// The on model creating.
        ///// </summary>
        ///// <param name="modelBuilder">
        ///// The model builder.
        ///// </param>
        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>().Property(e => e.Thread).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.Level).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.Logger).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.Url).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.Browser).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.User).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.Message).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.Exception).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.ExceptionType).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.ExceptionData).IsUnicode(false);

            modelBuilder.Entity<Log>().Property(e => e.AllXml).IsUnicode(false);

            modelBuilder.Entity<SystemSetting>().Property(e => e.UpdatedOn);
        }
    }
}