using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace InfluxDB.Tests
{
    //[TestFixture]
    //public class PerformanceTests
    //{
    //    [Test]
    //    [Ignore]
    //    public void PerformanceTest()
    //    {
    //        var client = new InfluxDbClient("192.168.1.100", 8086, "root", "root", "PerfTests");

    //        // Create a database for this test
    //        List<string> databaseList = client.GetDatabaseList();
    //        if (!databaseList.Contains("PerfTests"))
    //        {
    //            client.CreateDatabase("PerfTests");
    //        }

    //        client.Query("DROP SERIES foo");

    //        // Create 
    //        var serie = new Series {Name = "foo", ColumnNames = new[] {"value", "value_str"}};
    //        var series = new List<Series> {serie};

    //        const int N = 10000;
    //        for (int i = 0; i < N; i++)
    //        {
    //            serie.Points.Add(new object[] {i, "yoyo"});
    //        }

    //        // Measure insert
    //        Stopwatch chrono = Stopwatch.StartNew();
    //        client.Insert(series);
    //        chrono.Stop();
    //        Debug.Write("Insert Elapsed:" + chrono.Elapsed.TotalMilliseconds + " ms" + Environment.NewLine);

    //        // Ugly
    //        Thread.Sleep(1000); // Give some time to the database to process insert. There must be a better way to do this

    //        // Make sure write was succesful
    //        List<Series> result = client.Query("select count(value) from foo");
    //        Assert.AreEqual(N, result[0].Points[0][1]);

    //        // Measure query
    //        chrono.Restart();
    //        result = client.Query("select * from foo");
    //        chrono.Stop();
    //        Assert.AreEqual(N, result[0].Points.Count);
    //        Debug.Write("Query Elapsed:" + chrono.Elapsed.TotalMilliseconds + " ms" + Environment.NewLine);

    //        // Clean up
    //        client.DeleteDatabase("PerfTests");
    //    }

    //    [Test]
    //    [Ignore]
    //    public void SerieCreationPerformanceTest()
    //    {
    //        Stopwatch chrono = Stopwatch.StartNew();

    //        var serie = new Series {Name = "foo", ColumnNames = new[] {"value", "value_str"}};
    //        const long N = (long) 1e6;
    //        for (long i = 0; i < N; i++)
    //        {
    //            serie.Points.Add(new object[] {i, "some text"});
    //        }

    //        chrono.Stop();
    //        Debug.Write("Create Elapsed:" + chrono.Elapsed.TotalMilliseconds + " ms" + Environment.NewLine);
    //    }
    //}
}