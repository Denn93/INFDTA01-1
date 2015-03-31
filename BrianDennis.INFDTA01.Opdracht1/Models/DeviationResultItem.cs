using System;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class DeviationResultItem
    {
        public int I { get; set; }
        public int J { get; set; }
        public double Deviation { get; set; }
        public int UserCount { get; set; }

        public static DeviationResultItem Build(Tuple<int, int, double, int> items)
        {
            return new DeviationResultItem
            {
                I = items.Item1,
                J = items.Item2,
                Deviation = items.Item3,
                UserCount = items.Item4
            };
        }
    }
}