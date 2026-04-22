using BCrypt.Net;

namespace MiddleLayerAPI.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes the password inputted using BCrypt
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <returns>hashed password for storage</returns>
        public static string? HashPassword(string password)
        {
            if (password != null)
            {
                return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
            }
            return null;
        }   
        /// <summary>
        /// Verifies the inputted password is correct
        /// </summary>
        /// <param name="password">inputted password</param>
        /// <param name="hashedPassword">hashed password to compare against</param>
        /// <returns>True if they match, false otherwise</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }   


    }
}
