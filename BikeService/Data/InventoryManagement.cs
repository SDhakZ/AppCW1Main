using BikeService.Data;
using System.Text.Json;


namespace Inventory.Data;

public static class InventoryManagement
{
    // Serializes the given list of Inventory objects to a JSON string and writes it to a file on the local file system.
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
    // Reads a JSON string from a file on the local file system and deserializes it into a list of Inventory objects.
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

    /*Creates a new Inventory object with the given item name, quantity, and last taken out date, 
     * adds it to the list of existing Inventory objects, and saves the updated list to the file.*/
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
    // Deletes the Inventory object with the given unique identifier from the list of existing Inventory objects
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
    // Updates the item name and quantity of the Inventory object with the given unique identifier in the list of existing Inventory objects, and saves the updated list to the file
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
    // Updates the quantity of the inventory after requests and validates if the requested item is in stock, and if the required field is properly filled.
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
