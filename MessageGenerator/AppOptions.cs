namespace MessageGenerator
{
    public class AppOptions
    {
        public string HumidityJobCron { get; set; }

        public string PressureJobCron { get; set; }

        public string TemperatureJobCron { get; set; }

        public string WindJobCron { get; set; }

        public string RabbitMqUri { get; set; }
        
        public string Username { get; set; }

        public string Password { get; set; }
    }
}