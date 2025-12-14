using System;

namespace MileageTrackerMobile.Models;

public class LogItem
{
    public int Id { get; set; } 
    public DateTime Date { get; set; }
    public double Miles { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsGas { get; set; }

    public double? Gallons { get; set; }
    public double? PricePerGallon { get; set; }
    public int LogId { get; set; }
    public double? TotalCost => Gallons.HasValue && PricePerGallon.HasValue
        ? Gallons.Value * PricePerGallon.Value
        : null;
}