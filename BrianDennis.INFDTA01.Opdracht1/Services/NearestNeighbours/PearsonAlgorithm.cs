using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class PearsonAAlgorithm : AAlgorithm
    {
        public PearsonAAlgorithm(SortedDictionary<int, UserPreference> dataSet, string view) 
            : base(dataSet, view)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            Dictionary<int, float> targetUser = DataSet[targetUserId].Preferences;
            List<AlgorithmResultListItem> resultList = new List<AlgorithmResultListItem>();

            ThresHold = double.Parse(Configuration.Targets(View)["InitialThreshold"]);

            foreach (KeyValuePair<int, UserPreference> otherUser in DataSet)
            {
                if (otherUser.Key == targetUserId) continue;

                Dictionary<int, float> newwT =
                    targetUser.Where(m => otherUser.Value.Preferences.ContainsKey(m.Key))
                        .Select(m => m)
                        .OrderBy(m => m.Key)
                        .ToDictionary(m => m.Key, m => m.Value);

                Dictionary<int, float> newwO =
                    otherUser.Value.Preferences.Where(m => newwT.ContainsKey(m.Key))
                        .Select(m => m)
                        .OrderBy(m => m.Key)
                        .ToDictionary(m => m.Key, m => m.Value);

/*

                Dictionary<int, float> newTarget = targetUser.Where(m => otherUser.Value.ContainsMovie(m.MovieId))
                    .Select(m => m)
                    .OrderBy(m => m.MovieId)
                    .ToDictionary(m => m.MovieId, m => m.Rating);

                Dictionary<int, float> newOther = otherUser.Value.Where(m => newTarget.ContainsKey(m.MovieId))
                    .Select(m => m)
                    .OrderBy(m => m.MovieId)
                    .ToDictionary(m => m.MovieId, m => m.Rating);
*/

                KeyValuePair<int, float>[] targetTemp = newwT.ToArray();
                KeyValuePair<int, float>[] otherTemp = newwO.ToArray();

                double sumX = 0.00,
                    sumY = 0.00,
                    sumXSquare = 0.00,
                    sumYSquare = 0.00,
                    sumXy = 0.00;

                int index = 0;

                for (int i = 0; i < targetTemp.Length; i++)
                {
                    sumX += targetTemp[i].Value;
                    sumY += otherTemp[i].Value;
                    sumXSquare += (targetTemp[i].Value * targetTemp[i].Value);
                    sumYSquare += (otherTemp[i].Value * otherTemp[i].Value);
                    sumXy += (targetTemp[i].Value * otherTemp[i].Value);
                    index++;
                }

                double step1 = sumXy - (sumX*sumY)/index;
                double step2 = Math.Sqrt(sumXSquare - (Math.Pow(sumX, 2)/index));
                double step3 = Math.Sqrt(sumYSquare - (Math.Pow(sumY, 2)/index));

                double result = step1/(step2*step3);

                ResultAdd(resultList, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)), result);
            }

            return resultList;
        }
    }
}