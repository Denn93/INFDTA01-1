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

                Dictionary<int, float> newTarget =
                    targetUser.Where(m => otherUser.Value.Preferences.ContainsKey(m.Key))
                        .Select(m => m)
                        .OrderBy(m => m.Key)
                        .ToDictionary(m => m.Key, m => m.Value);

                Dictionary<int, float> newOther =
                    otherUser.Value.Preferences.Where(m => newTarget.ContainsKey(m.Key))
                        .Select(m => m)
                        .OrderBy(m => m.Key)
                        .ToDictionary(m => m.Key, m => m.Value);

                KeyValuePair<int, float>[] newTargetArray = newTarget.ToArray();
                KeyValuePair<int, float>[] newOtherArray = newOther.ToArray();

                CalculatePearson(resultList, newTargetArray, newOtherArray, targetUserId, otherUser.Key);
/*
                double sumX = 0.00,
                    sumY = 0.00,
                    sumXSquare = 0.00,
                    sumYSquare = 0.00,
                    sumXy = 0.00;

                int index = 0;

                for (int i = 0; i < newTargetArray.Length; i++)
                {
                    sumX += newTargetArray[i].Value;
                    sumY += newOtherArray[i].Value;
                    sumXSquare += (newTargetArray[i].Value * newTargetArray[i].Value);
                    sumYSquare += (newOtherArray[i].Value * newOtherArray[i].Value);
                    sumXy += (newTargetArray[i].Value * newOtherArray[i].Value);
                    index++;
                }

                double step1 = sumXy - (sumX*sumY)/index;
                double step2 = Math.Sqrt(sumXSquare - (Math.Pow(sumX, 2)/index));
                double step3 = Math.Sqrt(sumYSquare - (Math.Pow(sumY, 2)/index));

                double result = step1/(step2*step3);

                ResultAdd(resultList, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)), result);*/
            }

            return resultList;
        }

        /// <summary>
        /// Computes Pearson Algorithm by taking two KeyValuePairs that contain the ratings
        /// for both the target user and the other user and adds them to the result.
        /// </summary>
        /// <param name="resultList">The computed results (starts as empty list on first iteration)</param>
        /// <param name="newTargetArray">Target ratings</param>
        /// <param name="newOtherArray">Other user ratings</param>
        /// <param name="targetUserId"></param>
        /// <param name="otherUserId"></param>
        private void CalculatePearson(List<AlgorithmResultListItem> resultList, KeyValuePair<int, float>[] newTargetArray, KeyValuePair<int, float>[] newOtherArray, int targetUserId, int otherUserId)
        {
            double sumX = 0.00,
                    sumY = 0.00,
                    sumXSquare = 0.00,
                    sumYSquare = 0.00,
                    sumXy = 0.00;

            int index = 0;

            for (int i = 0; i < newTargetArray.Length; i++)
            {
                sumX += newTargetArray[i].Value;
                sumY += newOtherArray[i].Value;
                sumXSquare += (newTargetArray[i].Value * newTargetArray[i].Value);
                sumYSquare += (newOtherArray[i].Value * newOtherArray[i].Value);
                sumXy += (newTargetArray[i].Value * newOtherArray[i].Value);
                index++;
            }

            double numerator = sumXy - (sumX * sumY) / index;
            double denominatorLeft = Math.Sqrt(sumXSquare - (Math.Pow(sumX, 2) / index));
            double denominatorRight = Math.Sqrt(sumYSquare - (Math.Pow(sumY, 2) / index));

            double result = numerator / (denominatorLeft * denominatorRight);

            ResultAdd(resultList, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUserId, result, 0)), result);
        }
    }
}