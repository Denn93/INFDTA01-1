﻿using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class CosineAlgorithm : IAlgorithm
    {
        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            SortedDictionary<int, Dictionary<int, float>> dataSet = UserPreferenceService.DataSet;
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();
            Dictionary<int, float> targetUser = dataSet[targetUserId];

            ThresHold = Configuration.InitialThresHold;

            foreach (KeyValuePair<int, Dictionary<int, float>> user in dataSet)
            {
                if (user.Key == targetUserId) continue;
                if (targetUser.Count(m=>user.Value.ContainsKey(m.Key)) == 0) continue;


                KeyValuePair<int, float>[] targetUserArray = targetUser.OrderBy(m => m.Key).ToArray();
                Dictionary<int, float> otherUserArray = user.Value.OrderBy(m => m.Key).ToDictionary(m=>m.Key, m=>m.Value);

                List<float> targetUserRatings = new List<float>();
                List<float> otherUserRatings = new List<float>();

                for (int i = 0; i < targetUserArray.Count(); i++)
                {
                    targetUserRatings.Add(targetUserArray[i].Value);

                    otherUserRatings.Add(otherUserArray.ContainsKey(targetUserArray[i].Key)
                        ? otherUserArray[targetUserArray[i].Key]
                        : 0);
                }

                double similarity = GetCosineSimilarity(targetUserRatings, otherUserRatings);

                ResultAdd(result, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key, similarity, 0)), similarity);
            }
            
            return result;
        }

        public static double GetCosineSimilarity(List<float> targetList, List<float> otherList)
        {
            int count = ((otherList.Count < targetList.Count) ? otherList.Count : targetList.Count);
            
            double dot = 0.0d;
            double vectorLength1 = 0.0d;
            double vectorLength2 = 0.0d;

            for (int i = 0; i < count; i++)
            {
                dot += targetList[i] * otherList[i];
                vectorLength1 += Math.Pow(Math.Abs(targetList[i]), 2);
                vectorLength2 += Math.Pow(Math.Abs(otherList[i]), 2);
            }

            return dot / (Math.Sqrt(vectorLength1) * Math.Sqrt(vectorLength2));
        }

    }
}