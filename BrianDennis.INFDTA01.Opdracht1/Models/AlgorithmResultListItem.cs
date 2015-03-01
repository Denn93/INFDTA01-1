using System;

namespace BrianDennis.INFDTA01.Opdracht1.Models
{
    public class AlgorithmResultListItem
    {
        public int TargetUser { get; set; }
        public int OtherUser { get; set; }
        public double Similarity { get; set; }
        public int NotSimilarArticles { get; set; }

        public static AlgorithmResultListItem Build(Tuple<int, int, double, int> items)
        {
            return new AlgorithmResultListItem
            {
                TargetUser = items.Item1,
                OtherUser = items.Item2,
                Similarity = items.Item3,
                NotSimilarArticles = items.Item4
            };
        }
    }
}