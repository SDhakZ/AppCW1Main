using BikeService.Data;
using System.Text.Json;


namespace InventoryLog.Data;

public static class InventoryLogManagement
{
    // Serializes the given list of InventoryLog objects to a JSON string and writes it to a file on the local file system
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

    // Reads a JSON string from a file on the local file system and deserializes
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

    /* Creates a new InventoryLog object with the given user identifier, quantity taken, item name, and taken by name,
    adds it to the list of existing InventoryLog objects, and saves the updated list to the file. */
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
