influxdb-csharp
===============

C# client for Influxdb

Only very basic features are implemented so far.

```cs
var client = new InfluxDBClient("192.168.1.100", 8086, "root", "root", "MyDataBase");

// Create series
var serie = new Serie { Name = "foo", ColumnNames = new[] { "value", "value_str" } };
serie.Points.Add(new object[] { 1.0, "first" });
var series = new List<Serie> { serie };

// Insert series
client.Insert(series);

// Query database
List<Serie> result = client.Query("select * from foo");
```