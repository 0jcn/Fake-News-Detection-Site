using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiddleLayerAPI.Helpers
{
    /// <summary>
    /// Helper class for generating and decoding JWT tokens for user authentication and authorization
    /// </summary>
    public static class JWTHelper
    {
        /// <summary>
        /// Generate JWT token for when a user logs in
        /// </summary>
        /// <param name="userId">User ID from the database</param>
        /// <param name="username">Username of the user</param>
        /// <param name="tier">User tier or role</param>
        /// <param name="signingKey">Key used to sign the token</param>
        /// <returns>Generated JWT token as a string</returns>
        public static string GenerateToken(int userId, string username, string tier, string signingKey)
        {
            var claims = new[]
            {
                new Claim("userId", userId.ToString()),
                new Claim("username", username),
                new Claim(ClaimTypes.Role, tier)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// Decodes token to get the claims and validate the token using the signing key
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <param name="configKey">Key used to validate the token</param>
        /// <returns>ClaimsPrincipal if the token is valid, null otherwise</returns>
        public static ClaimsPrincipal DecodeToken(string token, string configKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                // Handle invalid token
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// Gets all claims from a token
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <param name="configKey">Key used to validate the token</param>
        /// <returns>Dictionary of claims if the token is valid, null otherwise</returns>
        public static Dictionary<string, object> GetClaimsFromToken(string token, string configKey)
        {
            var principal = DecodeToken(token, configKey);
            if (principal == null) return null;

            var claims = new Dictionary<string, object>();
            foreach (var claim in principal.Claims)
            {
                claims[claim.Type] = claim.Value;
            }
            return claims;
        }
        /// <summary>
        /// Retrieves the user id from the token claims, returns -1 if the token is invalid or does not contain a userId claim
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <param name="configKey">Key used to validate the token</param>
        /// <returns>User ID if the token is valid, -1 otherwise</returns>
        public static int GetUserIdFromToken(HttpRequest request, string configKey)
        {
            var token = GetTokenFromHeader(request);
            if (token != null) 
            { 
                var claims = GetClaimsFromToken(token, configKey);
                if (claims != null && claims.ContainsKey("userId"))
                {
                    return Convert.ToInt32(claims["userId"]);
                }
            }
            return -1; // Invalid user ID
        }
        /// <summary>
        /// Takes in the request and retrieves the token from the Authorization header
        /// </summary>
        /// <param name="request">HTTP request</param>
        /// <returns>JWT token if present</returns>
        public static string GetTokenFromHeader(HttpRequest request)
        {
            var token = request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            return token;

        }
    }
}
