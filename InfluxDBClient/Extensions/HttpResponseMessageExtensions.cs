using System.Net;
using System.Net.Http;

namespace InfluxDB.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        public static void ValidateHttpResponse(this HttpResponseMessage result, HttpStatusCode successStatusCode, bool treatOkAsFailure)
        {
            if (result.StatusCode == successStatusCode)
            {
                // Success!
            }
            else if (treatOkAsFailure && result.StatusCode == HttpStatusCode.OK)
            {
                throw new InfluxException("The request was understood but did not execute successfully: " + result.Content.ReadAsStringAsync(), 200);
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InfluxException("The request was incorrectly formatted: " + result.Content.ReadAsStringAsync(), 400);
            }
            else if ((int)result.StatusCode >= 500)
            {
                throw new InfluxException("An unexpected server error occurred: " + result.Content.ReadAsStringAsync(), (int)result.StatusCode);
            }
        }
    }
}
