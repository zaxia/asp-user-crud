using System.ComponentModel.DataAnnotations;
using BC = BCrypt.Net.BCrypt;

namespace base_test_mvc.Models;
public class User
{

    [Key]
    public string login { get; set; }
    public string password { get; set; }
    public DateTime createdAt{ get; set; }
    public DateTime updatedAt{ get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }

    public void encryptPassword()
    {
        this.password = BC.HashPassword(this.password);
    }

    public bool verifyPassword(string password)
    {
        return BC.Verify(password, this.password);
    }

    public static User generateRandom()
    {
        User user = new User();
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        user.login = new string(Enumerable.Repeat(chars, 10)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        string password = "mariana";
        user.password = BC.HashPassword(password);
        user.createdAt = DateTime.Now;
        user.updatedAt = DateTime.Now;
        return user;
    }
}
