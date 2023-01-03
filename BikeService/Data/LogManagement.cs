using BikeService.Data;
using System.Text.Json;


namespace Log.Data;

public static class LogManagement
{
    // Serializes the given list of Log objects to a JSON string and writes it to a file on the local file system.
    private static void SaveAll(List<Log> logs)
    {
        string appDataDirectoryPath = Utility.GetAppDirectoryPath();
        string logFilePath = Utility.GetLogFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(logs);
        File.WriteAllText(logFilePath, json);
    }
    // Reads a JSON string from a file on the local file system and deserializes it into a list of Log objects.
    public static List<Log> GetAll()
    {
        string logFilePath = Utility.GetLogFilePath();
        if (!File.Exists(logFilePath))
        {
            return new List<Log>();
        }

        var json = File.ReadAllText(logFilePath);

        return JsonSerializer.Deserialize<List<Log>>(json);
    }

    /* Creates a new Log object and determines action "Creation" or "Deletion"
    based on the action passed, and saves the updated list to the file.*/
    public static List<Log> CreateLog(Guid userId, string item, string action)
    {
       
        List<Log> logs = GetAll();

        logs.Add(new Log
        {
            Item = item,
            ActionPerformed= action,
            ActionPerformer=userId
        });
        SaveAll(logs);
        return logs;
    }
    
}
