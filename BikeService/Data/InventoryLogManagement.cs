using BikeService.Data;
using System.Text.Json;


namespace InventoryLog.Data;

public static class InventoryLogManagement
{
    private static void SaveAll(List<InventoryLog> inventoryLogs)
    {
        string appDataDirectoryPath = Utility.GetAppDirectoryPath();
        string inventoryLogFilePath = Utility.GetInventoryLogFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(inventoryLogs);
        File.WriteAllText(inventoryLogFilePath, json);
    }

    public static List<InventoryLog> GetAll()
    {
        string inventoryLogFilePath = Utility.GetInventoryLogFilePath();
        if (!File.Exists(inventoryLogFilePath))
        {
            return new List<InventoryLog>();
        }

        var json = File.ReadAllText(inventoryLogFilePath);

        return JsonSerializer.Deserialize<List<InventoryLog>>(json);
    }


    public static List<InventoryLog> Create(Guid userId,int quantityTaken, string item,string takenBy)
    {

        List<InventoryLog> inventoryLogs = GetAll();
        if (takenBy == "")
        {
            throw new Exception("Please enter the staff name");
        }

        inventoryLogs.Add(new InventoryLog
        {
            RequestedItem = item,
            QuantityTaken=quantityTaken,
            ApprovedBy = userId,
            TakenBy = takenBy,
            
        });

        SaveAll(inventoryLogs);
        return inventoryLogs;
    }
}
