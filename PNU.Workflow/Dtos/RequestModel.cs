using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Workflow.Dtos
{
    public class RequestModel
    {
        public int RequestId { get; set; }
        public string Title { get; set; }
        public string  Requester { get; set; }
        public DateTime CreateDate { get; set; }
        public string RequestStatus  { get; set; }
    }
}
