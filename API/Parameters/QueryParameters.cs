using System;

namespace API.Parameters;

public class QueryParameters
{
    public string OrderBy { get; set; }
    
    public DateTime ReadAfter { get; set; } = DateTime.MinValue;
    
    public DateTime ReadBefore { get; set; } = DateTime.MaxValue;
    
    public decimal MinValue { get; set; } = decimal.MinValue;

    public decimal MaxValue { get; set; } = decimal.MaxValue;
    
    public decimal MinSpeed { get; set; } = decimal.MinValue;
    
    public decimal MaxSpeed { get; set; } = decimal.MaxValue;

    public decimal MinDirection { get; set; } = decimal.MinValue;

    public decimal MaxDirection { get; set; } = decimal.MaxValue;
    
    public bool ValidDateRange => ReadBefore > ReadAfter;

    public bool ValidValueRange => MaxValue > MinValue;
    
    public bool ValidSpeedRange => MaxSpeed > MinSpeed;

    public bool ValidDirectionRange => MaxDirection > MinDirection;
}