using MiddleLayerAPI.Interfaces;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Helpers
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private AppDbContext _context;
        public DatabaseHelper(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Users> CreateUser(Users newUser)
        {

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

     
        public async Task<bool> DeleteUser(int userToDelete)
        {
            var user = await _context.Users.FindAsync(userToDelete);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Users?> GetUser(int userId)
        {
            Users? userToFind = await _context.Users.FindAsync(userId);
            
            return userToFind;
        }

        public Task<Users?> GetUserByUsername(string username)
        {
            Users? userToFind = _context.Users.FirstOrDefault(u => u.Username == username);
            return Task.FromResult(userToFind);
        }

        public async Task<bool> RemoveSavedDetection(int detectionid)
        {
            var detection = await _context.SavedDetections.FindAsync(detectionid);
            if (detection == null)
                return false;

            _context.SavedDetections.Remove(detection);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> SaveDetection(SavedDetections newDetection)
        {
            _context.SavedDetections.Add(newDetection);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Users?> UpdateUser(Users updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(updatedUser.Id);
            if (existingUser == null)
                return null;

            existingUser.Username = updatedUser.Username;
            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password;
            existingUser.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return await _context.Users.FindAsync(updatedUser.Id);
        }
    }
}
