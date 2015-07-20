namespace InfluxDB
{
    public class RetentionPolicy
    {
        internal RetentionPolicy(string name, Retention duration, int replication)
        {
            Name = name;
            Duration = duration;
            Replication = replication;
        }

        public string Name { get; private set; }
        public Retention Duration { get; private set; }
        public int Replication { get; private set; }
    }
}