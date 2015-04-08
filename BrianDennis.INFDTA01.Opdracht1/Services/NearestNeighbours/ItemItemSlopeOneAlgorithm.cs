using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemSlopeOneAlgorithm : AAlgorithm
    {
        public ItemItemSlopeOneAlgorithm(SortedDictionary<int, UserPreference> dataSet, string view)
            : base(dataSet, view)
        {}

        public Tuple<int, Dictionary<int, double>> SlopeOneResult  { get; set; }

        /// <summary>
        /// This method takes a targetUder and an optional targetItem. If a targetItem is given,
        /// only calculate the predicted ratings for that item. If no targetItem is given, calculate
        /// for all items.
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="targetItem"></param>
        public override void Calculate(int targetUser, int? targetItem)
        {
            Dictionary<Tuple<int, int>, Tuple<double, int>> deviations = ItemItemDeviationAlgorithm.GetDeviationResult(View);
            List<int> items = ItemItemDeviationAlgorithm.GetItemList(View);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            UserPreference userPreferences = DataSet[targetUser];
            Dictionary<int, double> predictedRatings = new Dictionary<int, double>();

            foreach (int i in items.Where(m => !userPreferences.Preferences.ContainsKey(m))) {
                if (targetItem != null) {
                    if (i == targetItem) {
                        CalculateSlopeOne(deviations, predictedRatings, userPreferences, i);
                        break;
                    }
                }
                else
                    CalculateSlopeOne(deviations, predictedRatings, userPreferences, i);
            }


            SlopeOneResult = new Tuple<int, Dictionary<int, double>>(targetUser, predictedRatings.OrderByDescending(m=>m.Value).Take(8).ToDictionary(m=>m.Key, m=>m.Value));
            watch.Stop();
            double elapsed = watch.Elapsed.TotalSeconds;
            System.Console.WriteLine();
        }

        private static void CalculateSlopeOne(Dictionary<Tuple<int, int>, Tuple<double, int>> deviations,
            Dictionary<int, double> predictedRatings, UserPreference userPreferences, int i)
        {
            double numerator = 0;
            double denominator = 0;

            foreach (KeyValuePair<int, float> item in userPreferences.Preferences) {
                Tuple<int, int> pair = new Tuple<int, int>(i, item.Key);

                if (deviations.ContainsKey(pair)) {
                    double deviation = deviations[pair].Item1;
                    int userCount = deviations[pair].Item2;

                    numerator += (item.Value + deviation)*userCount;
                    denominator += userCount;
                }
            }

            predictedRatings.Add(i, numerator/denominator);
        }
    }
}