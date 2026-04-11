using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Interfaces
{
    public interface IDatabaseHelper
    {
        public Task<Users> CreateUser(Users newUser);

        public Task<Users?> UpdateUser(UpdateUser updatedUser, int userId);

        public Task<bool> DeleteUser(int userToDelete);

        public Task<Users?> GetUser(int userId);

        public Task<bool> SaveDetection(SavedDetections newDetection);

        public Task<bool> RemoveSavedDetection(int detectionid);

        public Task<Users?> GetUserByUsername(string username);

        public Task<List<SavedDetections>> GetSavedDetectionsByUserId(int userId);
    }
}
