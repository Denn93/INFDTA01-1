using System.Collections.Generic;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class UserPreference 
    {
        /// <summary>
        /// This dictionary contains the movie /articleId and the rating. Int for Id, and float for the Rating
        /// </summary>
        public Dictionary<int, float> Preferences { get; set; } 
        public string Description { get; set; } 
        public string Genre { get; set; }
    }
}
