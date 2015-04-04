using System;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class DeviationUpdateModel
    {
        public Tuple<int, int> UpdatePair { get; set; }
        public int RatingI { get; set; }
        public int RatingJ { get; set; }
        public double OldDeviation { get; set; }
        public double NewDeviation{ get; set; }
    }
}