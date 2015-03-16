using System;
using System.Collections.Generic;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class PredictiveRatingViewModel
    {
        public Dictionary<int, double> Data { get; set; }
        public Tuple<int, string, string, int> CompleteData { get; set; }
    }
}