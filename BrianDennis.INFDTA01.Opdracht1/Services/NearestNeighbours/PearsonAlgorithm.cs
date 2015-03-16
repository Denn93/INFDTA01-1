using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class PearsonAAlgorithm : AAlgorithm
    {
        public PearsonAAlgorithm(SortedDictionary<int, Dictionary<int, float>> dataSet, string view) 
            : base(dataSet, view)
        {}

        public override List<AlgorithmResultListItem> Calculate(int targetUserId)
        {
            Dictionary<int, float> targetUser = DataSet[targetUserId];
            List<AlgorithmResultListItem> resultList = new List<AlgorithmResultListItem>();

            ThresHold = Configuration.InitialThresHold;

            foreach (KeyValuePair<int, Dictionary<int, float>> otherUser in DataSet)
            {
                if (otherUser.Key == targetUserId) continue;

                Dictionary<int, float> newTarget =
                    targetUser.Where(m => otherUser.Value.ContainsKey(m.Key))
                        .Select(m => m)
                        .OrderBy(m => m.Key)
                        .ToDictionary(m=>m.Key, m=>m.Value);

                Dictionary<int, float> newOther =
                    otherUser.Value.Where(
                        m =>
                            newTarget.ContainsKey(m.Key))
                        .Select(m => m)
                        .OrderBy(m => m.Key)
                        .ToDictionary(m => m.Key, m => m.Value);

                KeyValuePair<int, float>[] targetTemp = newTarget.ToArray();
                KeyValuePair<int, float>[] otherTemp = newOther.ToArray();

                double xi = 0;
                double xc = 0;
                double yi = 0;
                double yc = 0;
                double xiyi = 0;

                int index = newTarget.Count;

                for (int i = 0; i < targetTemp.Length; i++)
                {
                    xi = xi + targetTemp[i].Value;
                    xc = xc + (targetTemp[i].Value * targetTemp[i].Value);
                    yi = yi + otherTemp[i].Value;
                    yc = yc + (otherTemp[i].Value * otherTemp[i].Value);
                    xiyi = xiyi + (targetTemp[i].Value * otherTemp[i].Value);
                }

                double result = ((xiyi) - ((xi*yi)/index))/
                                (Math.Sqrt(xc - ((xi*xi)/index))*Math.Sqrt(yc - ((yi*yi)/index)));

                ResultAdd(resultList, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)), result);
                /*resultList.Add(AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)));*/
            }

            return resultList;
        }
    }
}