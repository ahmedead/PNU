// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     The log.
    /// </summary>
    [Table("common.Log")]
    internal class Log
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the application.
        /// </summary>
        [Required]
        [StringLength(60)]
        public string Application { get; set; }

        /// <summary>
        ///     Gets or sets the host.
        /// </summary>
        [StringLength(50)]
        public string Host { get; set; }

        /// <summary>
        ///     Gets or sets the date.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        ///     Gets or sets the thread.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Thread { get; set; }

        /// <summary>
        ///     Gets or sets the level.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Level { get; set; }

        /// <summary>
        ///     Gets or sets the logger.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Logger { get; set; }

        /// <summary>
        ///     Gets or sets the url.
        /// </summary>
        [StringLength(500)]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the status code.
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        ///     Gets or sets the browser.
        /// </summary>
        [StringLength(255)]
        public string Browser { get; set; }

        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        [StringLength(255)]
        public string User { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        [Required]
        [StringLength(4000)]
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the exception.
        /// </summary>
        [StringLength(6000)]
        public string Exception { get; set; }

        /// <summary>
        ///     Gets or sets the exception type.
        /// </summary>
        [StringLength(255)]
        public string ExceptionType { get; set; }

        /// <summary>
        ///     Gets or sets the exception data.
        /// </summary>
        [StringLength(500)]
        public string ExceptionData { get; set; }

        /// <summary>
        ///     Gets or sets the all xml.
        /// </summary>
        [StringLength(6000)]
        public string AllXml { get; set; }
    }
}