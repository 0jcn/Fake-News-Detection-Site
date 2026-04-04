using MiddleLayerAPI.Interfaces;
using MiddleLayerAPI.Models;

namespace MiddleLayerAPI.Helpers
{
    public class DatabaseHelper : IDatabaseHelper
    {

        public Users CreateUser(Users newUser, AppDbContext context)
        {
            throw new NotImplementedException();
        }

     
        public bool DeleteUser(Users userToDelete, AppDbContext context)
        {
            throw new NotImplementedException();
        }

        public Users GetUser(int userId, AppDbContext context)
        {
            throw new NotImplementedException();
        }

        public bool RemoveSavedDetection(int detectionid, AppDbContext context)
        {
            throw new NotImplementedException();
        }


        public bool SaveDetection(SavedDetections newDetection, AppDbContext context)
        {
            throw new NotImplementedException();
        }

        public Users UpdateUser(Users updatedUser, AppDbContext context)
        {
            throw new NotImplementedException();
        }
    }
}
