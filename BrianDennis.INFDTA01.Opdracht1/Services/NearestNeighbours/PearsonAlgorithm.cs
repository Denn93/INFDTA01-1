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

            foreach (KeyValuePair<int, Dictionary<int, float>> otherUser in dataSet)
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

  /*              double result = ((xiyi) - ((xi*yi)/index))/(Math.Sqrt(xc - ((xi)*xi)/index))*
                                Math.Sqrt(yc - ((yi)*yi)/index);                
  */
                double result = ((xiyi) - ((xi*yi)/index))/
                                (Math.Sqrt(xc - ((xi*xi)/index))*Math.Sqrt(yc - ((yi*yi)/index)));

/*

                float avgTarget = newTarget.Values.Average();
                float avgOther = newOther.Values.Average();

                float total1 =
                    newOther.Zip(newTarget,
                        (other1, target1) => (other1.Value - avgTarget)*(target1.Value - avgOther)).Sum();

                /*double subTotal1 = newOther.Values.Sum(x => Math.Pow((x - avgTarget), 2));
                double subTotal2 = newTarget.Values.Sum(x => Math.Pow((x - avgOther), 2));
#1#
                double totaltemp = 0.0d;
                double totalrating = Math.Pow(newOther.Values.Sum(), 2);
                foreach (var otherRow in newOther)
                {
                    double value = Math.Pow(otherRow.Value, 2);
                    totaltemp += value;
                }

                double totaltemp1 = 0.0d;
                double totalrating1 = Math.Pow(newTarget.Values.Sum(), 2);

                foreach (var targetRow in newTarget)
                {
                    double value = Math.Pow(targetRow.Value, 2);
                    totaltemp1 += value;
                }

                var subTotal1 = Math.Sqrt(totaltemp - (totalrating/newOther.Count));
                var subTotal2 = Math.Sqrt(totaltemp1 - (totalrating1 / newTarget.Count));

                double result = total1/(subTotal1*subTotal2);
                */
                /*ResultAdd(resultList, AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)), result);
                */resultList.Add(AlgorithmResultListItem.Build(new Tuple<int, int, double, int>(targetUserId, otherUser.Key, result, 0)));
            }

            return resultList;
        }
    }
}