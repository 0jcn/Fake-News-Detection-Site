using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Interfaces
{
    public interface DatabaseOperations
    {
        public Users CreateUser(Users newUser);

        public Users UpdateUser(Users updatedUser);

        public bool DeleteUser(Users userToDelete);

        public Users GetUser(int userId);

        public bool SaveDetection(SavedDetections newDetection);

        public bool RemoveSavedDetection(int detectionid);
    }
}
