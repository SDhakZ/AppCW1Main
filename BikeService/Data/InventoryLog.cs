using System.Data;

namespace InventoryLog.Data;

public class InventoryLog
{
    public string RequestedItem { get; set; }
    public int QuantityTaken { get; set; }
    public Guid ApprovedBy { get; set; }
    public string TakenBy { get; set; }
    public DateTime DateTakenOut { get; set; } = DateTime.Now;
}
