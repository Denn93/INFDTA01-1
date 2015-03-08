using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class PredictingRatingAlgorithm : AAlgorithm
    {
        public PredictingRatingAlgorithm(SortedDictionary<int, Dictionary<int, float>> dataSet) 
            : base(dataSet)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUser)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();
            Dictionary<int, float> targetArticles = DataSet[targetUser];

            double totalPearsonCoefficient = PearsonListData.Sum(m => m.Similarity);

            PredictiveRatings = new Dictionary<int, double>();

            foreach (AlgorithmResultListItem algorithmResultListItem in PearsonListData)
            {
                Dictionary<int, float> articles = DataSet[algorithmResultListItem.OtherUser];
                foreach (
                    KeyValuePair<int, float> article in
                        articles.Where(article => !targetArticles.ContainsKey(article.Key)))
                {
                    if (PredictiveRatings.ContainsKey(article.Key))
                    {
                        PredictiveRatings[article.Key] = PredictiveRatings[article.Key] +
                                                         CalculateWeightedRating(algorithmResultListItem.Similarity,
                                                             totalPearsonCoefficient, article.Value);
                    }
                    else
                        PredictiveRatings.Add(article.Key,
                            CalculateWeightedRating(algorithmResultListItem.Similarity, totalPearsonCoefficient,
                                article.Value));
                }
            }

            return result;
        }

        private static double CalculateWeightedRating(double pearson, double total, double rating)
        {
            return pearson/total*rating;
        }

        public List<AlgorithmResultListItem> PearsonListData { get; set; }
        public Dictionary<int, double> PredictiveRatings { get; set; }
    }
}