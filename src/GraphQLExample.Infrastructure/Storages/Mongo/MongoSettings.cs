namespace GraphQLExample.Infrastructure.Mongo
{
    public class MongoSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool Seed { get; set; }

        public bool RecreateDatabase { get; set; }

        public string DatabaseName { get; set; }

        public string Url()
            => $"{Host}:{Port}";
    }
}