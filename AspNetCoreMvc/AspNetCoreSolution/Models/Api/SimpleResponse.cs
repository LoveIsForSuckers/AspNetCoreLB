using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSolution.Models.Api
{
    public class SimpleResponse
    {
        private const string SUCCESS = "Success";
        private const string ERROR = "Error";

        static public object Success()
        {
            return new { Response = SUCCESS };
        }

        static public object Error(object details = null)
        {
            var obj = new { Response = ERROR, Details = details };
            return obj;
        }

        static public object Content(object content = null)
        {
            return new { Response = content };
        }
    }
}
