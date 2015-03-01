using System;
using System.Collections.Generic;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class EuclideanAlgorithm : IAlgorithm
    {
        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();

            SortedDictionary<int, Dictionary<int, float>> dataSet = UserPreferenceService.DataSet;
            Dictionary<int, float> target = dataSet[targetUserId];
            ThresHold = Configuration.InitialThresHold;

            foreach (KeyValuePair<int, Dictionary<int, float>> user in dataSet)
            {
                if (user.Key == targetUserId) continue;

                double distance = 0;
                int articlesNotFound = 0;
                foreach (KeyValuePair<int, float> preference in user.Value)
                {
                    if (!target.ContainsKey(preference.Key))
                    {
                        articlesNotFound ++;
                        continue;
                    }

                    float targetUserRating = target[preference.Key];
                    distance += Math.Pow(preference.Value - targetUserRating, 2);
                }

                double similarity = 1 / (1 + Math.Sqrt(distance));

                ResultAdd(result,
                    AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key, similarity,
                        articlesNotFound)), similarity);

/*
                if (similarity > thresHold)
                {
                    if (result.Count < 3)
                        result.Add(
                            AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key,
                                similarity, articlesNotFound)));
                    else
                    {
                
                        AlgorithmResultListItem lowest =
                            result.Aggregate((item1, item2) => item1.Similarity < item2.Similarity ? item1 : item2);

                        if (lowest.Similarity < similarity)
                        {
                            result.Remove(lowest);

                            result.Add(
                                AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key,
                                    similarity, articlesNotFound)));

                            thresHold = similarity;
                        }
                    }
                }*/
            }

            return result;
        }
    }
}