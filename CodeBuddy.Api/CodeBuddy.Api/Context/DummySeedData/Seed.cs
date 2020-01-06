using CodeBuddy.Api.Helpers;
using CodeBuddy.Api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBuddy.Api.Context.DummySeedData
{
    public class Seed
    {
        private readonly PasswordManager _passwordManager;

        public Seed(PasswordManager passwordManager)
        {
            this._passwordManager = passwordManager;
        }

        public void SeedUsers(DataContext context)
        {
            if (!context.Users.Any())
            {
                var userData = File.ReadAllText("Context/DummySeedData/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    _passwordManager.CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }
    }
}
