using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiddleLayerAPI.Models
{
    /// <summary>
    /// Model for interacting with the SavedDetections table in the database
    /// </summary>
    public class SavedDetections
    {
        [Key]
        [Column("detection_id")]
        public int DetectionId { get; set; }
        [ForeignKey("UserId")]
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("input")]
        public string Input { get; set; } = string.Empty;
        [Required]
        [Column("result")]
        public string? Result { get; set; }
        [Required]
        [Column("true_prob")]
        public float? TrueProbability { get; set; }
        [Required]
        [Column("barely_true_prob")]
        public float? BarelyTrueProbability { get; set; }
        [Required]
        [Column("half_true_prob")]
        public float? HalfTrueProbability { get; set; }
        [Required]
        [Column("false_prob")]
        public float? FalseProbability { get; set; }
        [Required]
        [Column("mostly_true_prob")]
        public float? MostlyTrueProbability { get; set; }
        [Required]
        [Column("pants_fire_prob")]
        public float? PantsFireProbability { get; set; }
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } 
    }
}
