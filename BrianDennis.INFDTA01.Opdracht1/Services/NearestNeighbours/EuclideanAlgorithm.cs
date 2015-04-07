using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class EuclideanAAlgorithm : AAlgorithm
    {
        public EuclideanAAlgorithm(SortedDictionary<int, UserPreference> dataSet, string view) 
            : base(dataSet, view)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();

            Dictionary<int, float> target = DataSet[targetUserId].Preferences;
            ThresHold = double.Parse(Configuration.Targets(View)["InitialThreshold"]);

            foreach (KeyValuePair<int, UserPreference> user in DataSet)
            {
                if (user.Key == targetUserId) continue;

                double distance = 0;
                int articlesNotFound = 0;

                foreach (KeyValuePair<int, float> preference in user.Value.Preferences)
                {
                    if (target.Count(m => m.Key == preference.Key) == 0)
                    {
                        articlesNotFound ++;
                        continue;
                    }

                    //This Linq expression gets the target user rating where the id is the same as the preference id. 
                    //The preference variable is a selected item for a user other than targetUser
                    float targetUserRating = target.Where(m => m.Key== preference.Key).Select(m => m.Value).First();
                    //Compute the distance by summing the squared differences of all items
                    distance += Math.Pow(preference.Value - targetUserRating, 2);
                }

                //Take the square root of the sum to compute the similarity and normalize by 1 divided by similarity.
                double similarity = 1/(1 + Math.Sqrt(distance));

                ResultAdd(result,
                    AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key, similarity,
                        articlesNotFound)), similarity);
            }

            return result;
        }
    }
}