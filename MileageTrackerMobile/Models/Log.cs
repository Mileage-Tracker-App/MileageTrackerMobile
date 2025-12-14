using System;
using System.Collections.Generic;
using System.Linq;

namespace MileageTrackerMobile.Models;

public class Log
{
    public int Id { get; set; }
    public string Vehicle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public int SessionId { get; set; }
    public List<LogItem> LogItems { get; set; } = new List<LogItem>();
    public int NumberOfLogItems => LogItems.Count;
    public double TotalMiles => LogItems.Sum(item => item.Miles);
}