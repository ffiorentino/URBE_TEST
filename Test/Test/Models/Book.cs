using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Book
    {
        public int id { get; set; }
        public int idRoom { get; set; }
        public int attendant { get; set; }
        public bool useProjector { get; set; }
        public bool useBlackboard { get; set; }
        public bool useWifi { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int status { get; set; }
    }
}
