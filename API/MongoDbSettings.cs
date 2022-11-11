namespace API
{
    public abstract class MongoDbSettings
    {
        public string ConnectionUri { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
    }

    public class MongoDbHumiditySettings : MongoDbSettings
    {
    };

    public class MongoDbPressureSettings : MongoDbSettings
    {
    };

    public class MongoDbTemperatureSettings : MongoDbSettings
    {
    };

    public class MongoDbWindSettings : MongoDbSettings
    {
    };
}