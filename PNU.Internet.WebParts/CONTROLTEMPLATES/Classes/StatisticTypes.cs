using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts
{
    public class StatisticTypes 
    {
        private StatisticTypes(string value) { Value = value; }

        public string Value { get; private set; }

        public static StatisticTypes CollegesInstitues { get { return new StatisticTypes("الكليات والمعاهد"); } }
        public static StatisticTypes Programs { get { return new StatisticTypes("البرامج الاكاديمية"); } }
        public static StatisticTypes Students { get { return new StatisticTypes("طالبة"); } }
        public static StatisticTypes Members { get { return new StatisticTypes("أعضاء هيئة التدريس"); } }

        public static StatisticTypes AllDepartments { get { return new StatisticTypes("قسم"); } }


        public override string ToString()
        {
            return Value;
        }

    }
}
