using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class PredictingRatingAlgorithm : AAlgorithm
    {
        public PredictingRatingAlgorithm(SortedDictionary<int, Dictionary<int, float>> dataSet, string view) 
            : base(dataSet, view)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUser)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();
            Dictionary<int, float> targetArticles = DataSet[targetUser];

            PredictiveRatings = new Dictionary<int, double>();
            Dictionary<int, Tuple<double, int>> articlesTotals = new Dictionary<int, Tuple<double, int>>();

            foreach (AlgorithmResultListItem algorithmResultListItem in PearsonListData)
            {
                Dictionary<int, float> articles = DataSet[algorithmResultListItem.OtherUser];

                foreach (KeyValuePair<int, float> article in articles)
                {
                    if (articlesTotals.ContainsKey(article.Key))
                        articlesTotals[article.Key] =
                            new Tuple<double, int>(
                                articlesTotals[article.Key].Item1 + algorithmResultListItem.Similarity,
                                articlesTotals[article.Key].Item2 + 1);
                    else
                    {
                        articlesTotals.Add(article.Key, new Tuple<double, int>(algorithmResultListItem.Similarity, 1));
                    }
                }
            }

            foreach (AlgorithmResultListItem algorithmResultListItem in PearsonListData)
            {
                Dictionary<int, float> articles = DataSet[algorithmResultListItem.OtherUser];

                foreach (
                    KeyValuePair<int, float> article in
                        articles.Where(article => !targetArticles.ContainsKey(article.Key)))
                {
                    if (View.Equals("MovieLens"))
                        if (!(articlesTotals[article.Key].Item2 > 2))
                            continue; 
    
                    if (PredictiveRatings.ContainsKey(article.Key))
                    {
                        PredictiveRatings[article.Key] = PredictiveRatings[article.Key] +
                                                         CalculateWeightedRating(algorithmResultListItem.Similarity,
                                                             articlesTotals[article.Key].Item1, article.Value);
                    }
                    else
                        PredictiveRatings.Add(article.Key,
                            CalculateWeightedRating(algorithmResultListItem.Similarity, articlesTotals[article.Key].Item1,
                                article.Value));
                }
            }

            return result;
        }

        private static double CalculateWeightedRating(double pearson, double total, double rating)
        {
            return (pearson/total)*rating;
        }

        public List<AlgorithmResultListItem> PearsonListData { get; set; }
        public Dictionary<int, double> PredictiveRatings { get; set; }
    }
}