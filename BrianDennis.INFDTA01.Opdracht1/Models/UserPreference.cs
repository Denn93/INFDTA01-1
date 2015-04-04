using System.Collections.Generic;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class UserPreference 
    {
        public int Id { get; set; }

        public Dictionary<int, float> Preferences { get; set; } 

        


        public string Description { get; set; } 
        public float  Rating { get; set; }
        public string Genre { get; set; }
    }
}
