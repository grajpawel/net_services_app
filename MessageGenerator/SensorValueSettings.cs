namespace MessageGenerator
{
    public class SensorValueSettings
    {
        public decimal From { get; set; }

        public decimal To { get; set; }
    }

    public class HumidityValueSettings : SensorValueSettings
    {
    }

    public class PressureValueSettings : SensorValueSettings
    {
    }

    public class TemperatureValueSettings : SensorValueSettings
    {
    }

    public class WindValueSettings : SensorValueSettings
    {
        public decimal DegreeFrom { get; set; }

        public decimal DegreeTo { get; set; }
    }
}