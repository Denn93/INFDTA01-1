using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class PearsonAlgorithm : IAlgorithm
    {
        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            SortedDictionary<int, Dictionary<int, float>> dataSet = UserPreferenceService.DataSet;
            Dictionary<int, float> targetUser = UserPreferenceService.DataSet[targetUserId];
            List<AlgorithmResultListItem> resultList = new List<AlgorithmResultListItem>();

            ThresHold = Configuration.InitialThresHold;

            float avgTarget = targetUser.Values.Average();

            foreach (KeyValuePair<int, Dictionary<int, float>> otherUser in dataSet)
            {
                if (otherUser.Key == targetUserId) continue;
                /*
                if (otherUser.Value.Count != targetUser.Count)
                    continue;*/

                float avgOther = otherUser.Value.Values.Average();

                float total1 =
                    otherUser.Value.Zip(targetUser,
                        (other1, target1) => (other1.Value - avgTarget)*(target1.Value - avgOther)).Sum();

                double subTotal1 = Math.Sqrt(otherUser.Value.Values.Sum(x => Math.Pow((x - avgTarget), 2)));
                double subTotal2 = Math.Sqrt(targetUser.Values.Sum(x => Math.Pow((x - avgOther), 2)));

                double result = total1/(subTotal1*subTotal2);
/*
                ResultAdd(resultList, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)), result);
                */resultList.Add(AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)));
            }

            return resultList;
        }
    }
}