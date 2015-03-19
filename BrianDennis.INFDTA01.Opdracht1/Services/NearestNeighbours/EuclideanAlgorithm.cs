using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class EuclideanAAlgorithm : AAlgorithm
    {
        public EuclideanAAlgorithm(SortedDictionary<int, List<UserPreference>> dataSet, string view) 
            : base(dataSet, view)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();

            List<UserPreference> target = DataSet[targetUserId];
            ThresHold = double.Parse(Configuration.Targets(View)["InitialThreshold"]);

            foreach (KeyValuePair<int, List<UserPreference>> user in DataSet)
            {
                if (user.Key == targetUserId) continue;

                double distance = 0;
                int articlesNotFound = 0;
                foreach (UserPreference preference in user.Value)
                {
                    if (target.Count(m => m.MovieId == preference.MovieId) == 0)
                    {
                        articlesNotFound ++;
                        continue;
                    }

                    float targetUserRating = target.Where(m => m.MovieId == preference.MovieId).Select(m => m.Rating).Single();

                    distance += Math.Pow(preference.Rating - targetUserRating, 2);
                }

                double similarity = 1/(1 + Math.Sqrt(distance));

                ResultAdd(result,
                    AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key, similarity,
                        articlesNotFound)), similarity);
            }

            return result;
        }
    }
}