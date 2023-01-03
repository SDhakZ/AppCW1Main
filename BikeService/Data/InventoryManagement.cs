using BikeService.Data;
using System.Text.Json;


namespace Inventory.Data;

public static class InventoryManagement
{
    private static void SaveAll(List<Inventory> inventories)
    {
        string appDataDirectoryPath = Utility.GetAppDirectoryPath();
        string inventoryFilePath = Utility.GetInventoryFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(inventories);
        File.WriteAllText(inventoryFilePath, json);
    }

    public static List<Inventory> GetAll()
    {
        string inventoryFilePath = Utility.GetInventoryFilePath();
        if (!File.Exists(inventoryFilePath))
        {
            return new List<Inventory>();
        }

        var json = File.ReadAllText(inventoryFilePath);

        return JsonSerializer.Deserialize<List<Inventory>>(json);
    }
 

    public static List<Inventory> Create(string item,int quantity, string lastTakenOut)
    {
        List<Inventory> inventories = GetAll();
  
        bool itemExists = inventories.Any(x=>x.Item== item);

        if (itemExists)
        {
            throw new Exception("Item already exists.");
        }
        else if (item == "")
        {
            throw new Exception("The field is empty please insert a value");
        }
        else if (quantity <= 0)
        {
            throw new Exception("The quantity needs to be greater than 0");
        }
        else
        {
            inventories.Add(new Inventory
            {
                Item = item,
                Quantity = quantity,
                LastTakenOut = lastTakenOut,

            });
            SaveAll(inventories);
            return inventories;
        }
       
    }

    public static List<Inventory> Delete(Guid id)
    {
   
        List<Inventory> inventories = GetAll();
        Inventory inventory = inventories.FirstOrDefault(x => x.Id == id);

        if (inventory == null)
        {
            throw new Exception("Inventory not found.");
        }

        inventories.Remove(inventory);
        SaveAll(inventories);
        return inventories;
    }

    public static List<Inventory> Update(Guid id, string item, int quantity)
    {
        List<Inventory> inventories = GetAll();
        Inventory inventoryToUpdate = inventories.FirstOrDefault(x => x.Id == id);
        DateTime date = DateTime.Now;

        if (item == "")
        {
            throw new Exception("The field is empty please insert a value");
        }
        else if (quantity < 0)
        {
            throw new Exception("The quantity must me greater than or equals to 0");
        }
        else
        {
            inventoryToUpdate.Item = item;
            inventoryToUpdate.Quantity = quantity;
            SaveAll(inventories);
            return inventories;
        }
    }

    public static List<Inventory> UpdateByRequest(Guid id,int quantity,string takenBy)
    {
        List<Inventory> inventories = GetAll();
        Inventory inventoryToUpdate = inventories.FirstOrDefault(x => x.Id == id);
        DateTime date = DateTime.Now;
        // Compares if the taken by name exists in staff names.
        var checkStaffName= UserManagement.GetAll().Where(t => t.Username.ToLower().Equals(takenBy.ToLower()) && t.UserRole==UserRole.Staff);
        if (takenBy == "")
        {
            throw new Exception("The field is empty");
        }
        if (quantity <= 0)
            {
                throw new Exception("The quantity must me greater than 0");
            }
        if (inventoryToUpdate.Quantity == 0)
            {
                throw new Exception("The item is out of stock");
            }

        if (quantity > inventoryToUpdate.Quantity)
            {
                throw new Exception("Requested item quantity exceeds stock quantity");
            }
        if (!checkStaffName.Any()){
            throw new Exception("The staff username does not exist");
        }
        inventoryToUpdate.Quantity -= quantity;
        inventoryToUpdate.LastTakenOut = date.ToString("MMM dd, yyyy h:mm tt");
        SaveAll(inventories);
        return inventories;
    }

}
