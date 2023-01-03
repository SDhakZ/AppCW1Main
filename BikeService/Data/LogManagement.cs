using BikeService.Data;
using System.Text.Json;


namespace Log.Data;

public static class LogManagement
{
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


    public static List<Log> CreationLog(Guid userId, string item)
    {
       
        List<Log> logs = GetAll();

        logs.Add(new Log
        {
            Item = item,
            ActionPerformed= "Creation",
            ActionPerformer=userId
        });
        SaveAll(logs);
        return logs;
    }
    
    public static List<Log> DeletionLog(Guid userId, string item)
    {
        List<Log> logs = GetAll();

        logs.Add(new Log
        {
            Item = item,
            ActionPerformed = "Deletion",
            ActionPerformer = userId
        });
        SaveAll(logs);
        return logs;
    }

}
