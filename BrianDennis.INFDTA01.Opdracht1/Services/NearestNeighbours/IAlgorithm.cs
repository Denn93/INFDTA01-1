using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public abstract class IAlgorithm
    {
        protected double ThresHold { get; set; }

        public abstract List<AlgorithmResultListItem> Calculate(int targetUser);

        protected void ResultAdd(List<AlgorithmResultListItem> list, AlgorithmResultListItem item, double similarity)
        {
            if (similarity > ThresHold)
            {
                if (list.Count < 3)
                    list.Add(item);
                else
                {
                    AlgorithmResultListItem lowest =
                        list.Aggregate((item1, item2) => item1.Similarity < item2.Similarity ? item1 : item2);

                    if (lowest.Similarity < similarity)
                    {
                        list.Remove(lowest);
                        list.Add(item);

                        ThresHold = similarity;
                    }
                }
            }
        }
    }
}