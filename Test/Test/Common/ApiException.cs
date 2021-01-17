using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Test.Common
{
    public class ApiException : Exception
    {
        public HttpStatusCode statusCode { get; set; }
        public string message { get; set; }

        public string errorCode { get; set; }

        public string stackTrace { get; set; }

        public string severity { get; set; }

        public string parameters { get; set; }
    }
}
