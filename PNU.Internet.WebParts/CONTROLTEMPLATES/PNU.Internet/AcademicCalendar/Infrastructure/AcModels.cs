using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls
{
    /// <summary>
    /// Flat DTO for one academic-calendar row. Language selection AND HTML encoding
    /// happen at projection time (AcSectionBase.Project), so the ASCX only needs simple
    /// Eval() calls inside the Repeater ItemTemplate - the Procedure/Week/... values are
    /// already the correct language for the page and HTML-encoded.
    /// </summary>
    public class AcRow
    {
        public int Id { get; set; }

        /// <summary>الإجراء - the procedure name, resolved to the page language.</summary>
        public string Procedure { get; set; }

        /// <summary>الأسبوع</summary>
        public string Week { get; set; }

        /// <summary>اليوم/الفترة</summary>
        public string DayPeriod { get; set; }

        /// <summary>التاريخ الهجري</summary>
        public string HijriDate { get; set; }

        /// <summary>التاريخ الميلادي</summary>
        public string GregorianDate { get; set; }

        /// <summary>Semester group (stored choice value - kept raw for grouping/matching).</summary>
        public string Semester { get; set; }

        public int ItemOrder { get; set; }
    }

    /// <summary>
    /// One accordion panel of the calendar: a semester heading plus its rows.
    /// Built in the code-behind before binding, so the markup stays declarative.
    /// HeadingId / CollapseId are computed per group so the Bootstrap accordion
    /// data-bs-target / aria-controls wiring is unique and stable.
    /// </summary>
    public class AcSemesterGroup
    {
        public string Title { get; set; }
        public string HeadingId { get; set; }
        public string CollapseId { get; set; }
        public List<AcRow> Items { get; set; }

        public AcSemesterGroup() { Items = new List<AcRow>(); }
    }

    /// <summary>
    /// Section header text, read from the AcCalendarHeaders list (one row) and resolved
    /// to the page language. Values are RAW (not encoded) - the control encodes them.
    /// </summary>
    public class AcHeader
    {
        public string Heading { get; set; }
        public string Intro { get; set; }
        public string MoeText { get; set; }
        public string MoeUrl { get; set; }

        public bool HasHeading { get { return !string.IsNullOrEmpty(Heading); } }
        public bool HasIntro { get { return !string.IsNullOrEmpty(Intro); } }
        public bool HasMoe { get { return !string.IsNullOrEmpty(MoeUrl); } }
    }
}
