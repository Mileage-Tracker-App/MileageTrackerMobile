using System;
using System.Collections.Generic;

namespace MileageTrackerMobile.Models;

public class Session
{
    public int Id { get; set; }
    public List<Log> Logs { get; set; } = new List<Log>();
}

public class TSession
{
    public int Id { get; set; }
    public List<Log> Logs { get; set; } = new List<Log>();
}