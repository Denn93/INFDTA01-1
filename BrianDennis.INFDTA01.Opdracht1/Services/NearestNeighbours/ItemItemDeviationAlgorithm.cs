using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Extensions;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemDeviationAlgorithm : AAlgorithm
    {
        public ItemItemDeviationAlgorithm(SortedDictionary<int, List<UserPreference>> dataSet, string view) : base(dataSet, view)
        {}


        public override List<AlgorithmResultListItem> Calculate(int targetUser)
        {
            int[] targetpair = {103, 106};

            SortedDictionary<int, List<UserPreference>> dataSet =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.userItemCsv);

            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();

   /*         foreach (KeyValuePair<int, List<UserPreference>> keyValuePair in dataSet)
            {
                double currentDeviation = 0;
                int userCount = 0;

                if (keyValuePair.Value.ContainsMovie(targetpair[0]) && keyValuePair.Value.ContainsMovie(targetpair[1]))
                {
                    float ratingItem1 = keyValuePair.Value.Single(m => m.MovieId == targetpair[0]).Rating;
                    float ratingItem2 = keyValuePair.Value.Single(m => m.MovieId == targetpair[1]).Rating;

                    currentDeviation += ratingItem1 - ratingItem2;
                    userCount ++;
                } 
            }

            currentDeviation = currentDeviation / userCount;

            result.Add(new AlgorithmResultListItem
            {
                TargetUser = userCount,
                Similarity = currentDeviation
            });*/

            return result;
        }
    }
}