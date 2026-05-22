using System.Net;

namespace Jornadas_Metalurgia_2026.Utils
{
    public class HttpResponseError : Exception
    {
       
        public HttpStatusCode StatusCode { get; set; }

        public HttpResponseError(HttpStatusCode statusCode, string message) : base(message)
        {
            
            StatusCode = statusCode;
        }
    }
}
