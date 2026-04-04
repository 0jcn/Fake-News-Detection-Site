namespace MiddleLayerAPI.Models
{
    public class SavedDetections
    {
        public int DetectionId { get; set; }    

        public int UserId { get; set; } 

        public string Input { get; set; } = string.Empty;

        public float? Result { get; set; }
        
        public float? TrueProbability { get; set; }

        public float? BarelyTrueProbability { get; set; }

        public float? HalfTrueProbability { get; set; } 

        public float? FalseProbability { get; set; }

        public float? MostlyTrueProbability { get; set; }   

        public float? PantsFireProbability { get; set; }    
    }
}
