using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using CodeBuddy.Api.Model;
using Microsoft.EntityFrameworkCore;
using CodeBuddy.Api.Helpers;

namespace CodeBuddy.Api.Context.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly PasswordManager _passwordManager;

        public AuthRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
            this._passwordManager = new PasswordManager();
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            _passwordManager.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == userName);

            if (user == null)
            {
                return null;
            }

            if (!_passwordManager.VerifyPasswordHash(password , user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<bool> IsUserExist(string userName)
        {
            if (await _dataContext.Users.AnyAsync(x => x.Username == userName))
            {
                return true;
            } 
            
            return false;
        }        
    }
}
