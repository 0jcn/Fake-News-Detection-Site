using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Interfaces
{
    public interface IDatabaseHelper
    {
        public Users CreateUser(Users newUser, AppDbContext context);

        public Users UpdateUser(Users updatedUser, AppDbContext context);

        public bool DeleteUser(Users userToDelete, AppDbContext context);

        public Users GetUser(int userId, AppDbContext context);

        public bool SaveDetection(SavedDetections newDetection, AppDbContext context);

        public bool RemoveSavedDetection(int detectionid, AppDbContext context);
    }
}
