using System;
using System.Linq;
using InfluxDB.Response;

namespace InfluxDB.Extensions
{
    public static class ResultExtensions
    {
        public static Response.Series GetSingleSeries(this Result result)
        {
            Response.Series firstSeries;

            try
            {
                firstSeries = result.Series.SingleOrDefault();

                if (firstSeries == null)
                {
                    throw new InfluxException("Result did not contain a Series");
                }
            }
            catch (InvalidOperationException ioe)
            {
                throw new InfluxException("Result contained an unexpected number of Series", ioe);
            }

            return firstSeries;
        }

        public static void EnsureSuccess(this Result result)
        {
            if (!string.IsNullOrEmpty(result.Error))
            {
                throw new InfluxException(result.Error);
            }
        }
    }
}