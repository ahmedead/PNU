using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Jobs.Dtos
{
   public class JobDto
    {
        public int ID { get; set; }
        public Guid UniqueId { get; set; }
        public string  JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string  JobType { get; set; }
        public string JobCategory { get; set; }
        public string JobLocation { get; set; }
        public DateTime AppliedDueDate { get; set; }
        public DateTime JobDate { get; set; }
       

    }

    public class JobCondition
    {
        public string Title { get; set; }
    }
    public class JobDocument
    {
        public string Title { get; set; }
    }
}
