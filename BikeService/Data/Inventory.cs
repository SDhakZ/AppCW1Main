using System.Data;

namespace Inventory.Data;

public class Inventory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Item { get; set; }
    public int Quantity { get; set; }
    public string LastTakenOut { get; set; }

}
