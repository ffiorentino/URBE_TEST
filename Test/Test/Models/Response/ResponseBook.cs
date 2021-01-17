using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Common;

namespace Test.Models.Response
{
    public class ResponseBook
    {
        public MessageResponse message { get; set; }
        public bool success { get; set; }
    }
}
