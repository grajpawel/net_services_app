using System;

namespace Dashboard.Dtos;

public interface ISensorDto
{

    public string type { get; set; }
    
    public int SensorId { get; set; }
    
    public DateTime Time { get; set; }
    
    public decimal Value { get; set; }
    
    public decimal Direction { get; set; }
}