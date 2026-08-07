using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Workflow.Dtos
{
    public class RequestDetailsModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string MediaContent { get; set; }
        public string MainCategory { get; set; }
        public string FacultyName { get; set; }
        public string MediaImage { get; set; }
        public List<string> MediaTypes { get; set; }
        public DateTime MediaDate { get; set; }
    }
}
