namespace MiddleLayerAPI.Models
{
    /// <summary>
    /// Simple model for updating user information.
    /// </summary>
    public class UpdateUser
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}
