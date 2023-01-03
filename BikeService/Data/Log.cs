using System.Data;

namespace Log.Data;

public class Log
{
    public string Item { get; set; }
    public DateTime ItemDate { get; set; } = DateTime.Now;
    public string ActionPerformed { get; set; }
    public Guid ActionPerformer { get; set; }
}
