using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Room
    {
        public int id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public bool projector { get; set; }
        public bool blackboard { get; set; }
        public bool wifi { get; set; }
    }
}
