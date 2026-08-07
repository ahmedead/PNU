using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.Classes
{
    public class ContentEditorUsers
    {
        public int Id { get; set; } = 0;
        public string UserName { get; set; }
        public string UserEmail { get; set; }        
        public string DeptTitle { get; set; }
        public bool CanDelete { get; set; }
        public string DeptCode { get; set; }

    }

}
