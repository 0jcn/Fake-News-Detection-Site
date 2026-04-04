using MiddleLayerAPI.Interfaces;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Helpers
{
    public class DatabaseHelper : IDatabaseHelper
    {
        public Users CreateUser(Users newUser)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(Users userToDelete)
        {
            throw new NotImplementedException();
        }

        public Users GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSavedDetection(int detectionid)
        {
            throw new NotImplementedException();
        }

        public bool SaveDetection(SavedDetections newDetection)
        {
            throw new NotImplementedException();
        }

        public Users UpdateUser(Users updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
