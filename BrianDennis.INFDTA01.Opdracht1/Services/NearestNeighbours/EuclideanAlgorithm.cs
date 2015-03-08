﻿using System;
using System.Collections.Generic;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class EuclideanAAlgorithm : AAlgorithm
    {
        public EuclideanAAlgorithm(SortedDictionary<int, Dictionary<int, float>> dataSet) 
            : base(dataSet)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            List<AlgorithmResultListItem> result = new List<AlgorithmResultListItem>();

            Dictionary<int, float> target = DataSet[targetUserId];
            ThresHold = Configuration.InitialThresHold;

            foreach (KeyValuePair<int, Dictionary<int, float>> user in DataSet)
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

                double similarity = 1/(1 + Math.Sqrt(distance));

                ResultAdd(result,
                    AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, user.Key, similarity,
                        articlesNotFound)), similarity);
            }

            return result;
        }
    }
}