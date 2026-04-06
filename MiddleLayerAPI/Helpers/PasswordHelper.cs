using BCrypt.Net;

namespace MiddleLayerAPI.Helpers
{
    public static class PasswordHelper
    {
        public static string? HashPassword(string password)
        {
            if (password != null)
            {
                return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            }
            return null;
        }   

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }   


    }
}
