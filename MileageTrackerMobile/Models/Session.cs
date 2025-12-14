using System.Collections.Generic;

namespace MileageTrackerMobile.Models;

public class Session
{
    public int Id { get; set; }
    public List< Log> Logs { get; set; } = new List<Log>();
}