using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pnu.Internet.CustomTimerJobs.Statistics
{
    public class OldStatisticTypes 
    {
        private OldStatisticTypes(string value) { Value = value; }

        public string Value { get; private set; }

        public static OldStatisticTypes CollegesInstitues { get { return new OldStatisticTypes("الكليات والمعاهد"); } }
        public static OldStatisticTypes Programs { get { return new OldStatisticTypes("البرامج الاكاديمية"); } }
        public static OldStatisticTypes Students { get { return new OldStatisticTypes("طالبة"); } }
        public static OldStatisticTypes Members { get { return new OldStatisticTypes("أعضاء هيئة التدريس"); } }



        public override string ToString()
        {
            return Value;
        }

    }
}
