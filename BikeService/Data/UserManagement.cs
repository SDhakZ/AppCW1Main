
using System.Data;
using System.Diagnostics;
using System.Text.Json;

namespace BikeService.Data;

public static class UserManagement
{
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

    // Serializes the given list of Users objects to a JSON string and writes it to a file on the local file system.
    private static void SaveAll(List<User> users)
    {
        string appDataDirectoryPath = Utility.GetAppDirectoryPath();
        string appUsersFilePath = Utility.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
      
    }
    // Reads a JSON string from a file on the local file system and deserializes it into a list of User objects.
    public static List<User> GetAll()
    {
        string appUsersFilePath = Utility.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);
        return JsonSerializer.Deserialize<List<User>>(json);
    }

    // This method adds a new User object to the list of Users and saves the updated list to the file in the application's data directory.
    // It also checks admin count. 
    public static List<User> Create(Guid userId, string username, string password, UserRole UserRole)
    {
        List<User> users = GetAll();
        bool usernameExists = users.Any(x => x.Username == username);
        var adminCount = users.Count(x => x.UserRole == UserRole.Admin);

        if (usernameExists)
        {
            throw new Exception("Username already exists. Please enter another username");
        }
        if (UserRole == UserRole.Admin)
        {
            //Check admin count
            if (adminCount >= 2)
            {
                throw new Exception("Admin count limit exceeded");
            }
        }

        if (username == "")
        {
            throw new Exception("Please fill in the username field");
        }

        if (password == "")
        {
            throw new Exception("Please fill in the password field");
        }

        users.Add(
            new User
            {
                Username = username,
                PasswordHash = Utility.HashSecret(password),
                UserRole = UserRole,
                CreatedBy = userId
            }
        );
            SaveAll(users);
            return users;      
    }

    // This method helps to seed user when there are no user at the beginning of the appllication.
    public static void SeedUsers()
    {
        var users = GetAll().FirstOrDefault(x => x.UserRole == UserRole.Admin);

        if (users == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, UserRole.Admin);
        }
    }

    public static User GetById(Guid id)
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }


    // This method verifies the username and also the password (by checking hash) on login.
    public static User Login(string username, string password)
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utility.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }

        return user;
    }

    // This method hels to remove the user from list of users and save it.
    public static List<User> Delete(Guid id)
    {
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        users.Remove(user);
        SaveAll(users);

        return users;
    }
}
