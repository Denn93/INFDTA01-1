using System;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class DeviationUpdateModel
    {
        public Tuple<int, int> UpdatePair { get; set; }
        public float RatingI { get; set; }
        public float RatingJ { get; set; }
        public double OldDeviation { get; set; }
        public double NewDeviation{ get; set; }
    }
}