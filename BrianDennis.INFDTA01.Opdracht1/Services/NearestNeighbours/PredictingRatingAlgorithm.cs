using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Extensions;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class PredictingRatingAlgorithm : AAlgorithm
    {
        public PredictingRatingAlgorithm(SortedDictionary<int, List<UserPreference>> dataSet, string view) 
            : base(dataSet, view)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUser)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();
            List<UserPreference> targetArticles = DataSet[targetUser];

            PredictiveRatings = new Dictionary<int, double>();
            Dictionary<int, Tuple<double, int>> articlesTotals = new Dictionary<int, Tuple<double, int>>();

            foreach (AlgorithmResultListItem algorithmResultListItem in PearsonListData)
            {
                List<UserPreference> articles = DataSet[algorithmResultListItem.OtherUser];

                foreach (UserPreference article in articles)
                {
                    if (articlesTotals.ContainsKey(article.MovieId))
                        articlesTotals[article.MovieId] =
                            new Tuple<double, int>(
                                articlesTotals[article.MovieId].Item1 + algorithmResultListItem.Similarity,
                                articlesTotals[article.MovieId].Item2 + 1);
                    else
                    {
                        articlesTotals.Add(article.MovieId, new Tuple<double, int>(algorithmResultListItem.Similarity, 1));
                    }
                }
            }

            foreach (AlgorithmResultListItem algorithmResultListItem in PearsonListData)
            {
                List<UserPreference> articles = DataSet[algorithmResultListItem.OtherUser];

                foreach (
                    UserPreference article in
                        articles.Where(article => !targetArticles.ContainsMovie(article.MovieId)))
                {
                    if (View.Equals("MovieLens"))
                        if (!(articlesTotals[article.MovieId].Item2 > 2))
                            continue; 
    
                    if (PredictiveRatings.ContainsKey(article.MovieId))
                    {
                        PredictiveRatings[article.MovieId] = PredictiveRatings[article.MovieId] +
                                                         CalculateWeightedRating(algorithmResultListItem.Similarity,
                                                             articlesTotals[article.MovieId].Item1, article.Rating);
                    }
                    else
                        PredictiveRatings.Add(article.MovieId,
                            CalculateWeightedRating(algorithmResultListItem.Similarity, articlesTotals[article.MovieId].Item1,
                                article.Rating));
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