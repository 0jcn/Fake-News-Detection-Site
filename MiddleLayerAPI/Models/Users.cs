using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiddleLayerAPI.Models
{
    /// <summary>
    /// Model for interacting with the Users table in the database
    /// </summary>
    public class Users
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("username")]
        public string? Username { get; set; }
        [Required]
        [Column("password")]
        public string? Password { get; set; }
        [Required]
        [Column("email")]
        public string? Email { get; set; }
        [Required]
        [Column("tier")]
        public string? Tier { get; set; }
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
