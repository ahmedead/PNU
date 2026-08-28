using System;
using System.Collections.Generic;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.ProfessionalCertifications.Controls
{
    public class PcBreadcrumbItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsLast { get; set; }
    }

    public class PcHeaderModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Paragraph2 { get; set; }
        public string ExportButtonText { get; set; }
        public List<PcBreadcrumbItem> BreadcrumbItems { get; set; }

        public PcHeaderModel()
        {
            BreadcrumbItems = new List<PcBreadcrumbItem>();
        }
    }

    public class PcCertificationModel
    {
        public int id { get; set; }
        public string college { get; set; }
        public string department { get; set; }
        public string Program { get; set; }
        public string title_ar { get; set; }
        public string title_en { get; set; }
        public string description { get; set; }
        public string level { get; set; }
        public string language { get; set; }
        public string cost { get; set; }
        public string mode { get; set; }
        public string is_supported { get; set; }
        public string topics { get; set; }
        public string requirements { get; set; }
        public string registration_steps { get; set; }
        public string provider { get; set; }
        public string resources { get; set; }
        public string validity { get; set; }
        public string exam_duration { get; set; }
        public string questions_count { get; set; }
        public string questions_type { get; set; }
        public string ContactPerson { get; set; }
        public string course_1 { get; set; }
        public string course_2 { get; set; }
        public string course_3 { get; set; }
        public string course_4 { get; set; }
        public string link { get; set; }
        public int item_order { get; set; }
    }
}
