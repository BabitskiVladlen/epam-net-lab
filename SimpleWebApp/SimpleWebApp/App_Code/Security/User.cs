using System.Collections.Generic;

namespace SimpleWebApp.Security
{
    public static class Users
    {
         public static List<User> AllUsers
         {
             get
             {
                 List<User> users = new List<User>();
                 users.Add(new User() { UserName = "Vladlen", Password = "d7ef0dc51696e24033b3f0526b787202" });
                 return users;
             }
         }

    }

    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}