using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using kennards.generic.classes;

namespace kennard.gomoku.xUnit
{
   
    public class TestFactory
    {
       
           
       

        public static HttpRequest CreateHttpRequest(Dictionary<string,StringValues> parameters)

        {
            var context = new DefaultHttpContext();
            var request = context.Request;
            request.Query = new QueryCollection(parameters);
            return request;
        }

        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
