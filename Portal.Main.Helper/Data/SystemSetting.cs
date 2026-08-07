// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemSetting.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The system setting.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Data
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    ///     The system setting.
    /// </summary>
    [Table("common.SystemSettings")]
    internal class SystemSetting
    {
        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the application id.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ApplicationID { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether secure.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Key { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Value { get; set; }

        /// <summary>
        ///     Gets or sets the created by.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string CreatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the created on.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        ///     Gets or sets the updated by.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string UpdatedBy { get; set; }

        /// <summary>
        ///     Gets or sets the updated on.
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime UpdatedOn { get; set; }
    }
}