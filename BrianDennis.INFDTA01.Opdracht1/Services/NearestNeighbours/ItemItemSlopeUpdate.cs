using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemSlopeUpdate : AAlgorithm
    {
        public ItemItemSlopeUpdate(SortedDictionary<int, UserPreference> dataSet, string view) 
            : base(dataSet, view)
        {}

        public List<DeviationUpdateModel> ResultModel{ get; set; }
        
        /// <summary>
        /// Given a target user, item and rating, it adds the rating to the the user and
        /// updates the deviations for the pairs that include the given item.
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="targetItem"></param>
        /// <param name="ratingI"></param>
        public override void Calculate(int targetUser, int targetItem, float ratingI)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            ResultModel = new List<DeviationUpdateModel>();
            Dictionary<Tuple<int, int>, Tuple<double, int>> deviations = ItemItemDeviationAlgorithm.GetDeviationResult(View);

            SortedDictionary<int, UserPreference> dataset =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.userItemCsv);
            dataset[targetUser].Preferences.Add(targetItem, ratingI);

            List<Tuple<int, int>> updatePairs =
                ItemItemDeviationAlgorithm.GetPairList(View).Where(m => m.Item1 == targetItem || m.Item2 == targetItem).ToList();

            foreach (Tuple<int, int> updatePair in updatePairs)
            {
                if (!dataset[targetUser].Preferences.ContainsKey(updatePair.Item1) || !dataset[targetUser].Preferences.ContainsKey(updatePair.Item2))
                    continue;

                float ratingJ = (updatePair.Item1 != targetItem)
                    ? dataset[targetUser].Preferences[updatePair.Item1]
                    : dataset[targetUser].Preferences[updatePair.Item2];

                double currentDeviation = deviations[updatePair].Item1;
                double newCurrentDeviation = ((deviations[updatePair].Item1*deviations[updatePair].Item2) +
                                              (ratingI - ratingJ)) /
                                             (deviations[updatePair].Item2 + 1);

                deviations[updatePair] = new Tuple<double, int>(newCurrentDeviation, deviations[updatePair].Item2 + 1);
                deviations[FlipPair(updatePair)] = new Tuple<double, int>(FlipResult(newCurrentDeviation), deviations[FlipPair(updatePair)].Item2 + 1);

                ResultModel.Add(new DeviationUpdateModel
                {
                    UpdatePair = updatePair,
                    NewDeviation = newCurrentDeviation,
                    OldDeviation = currentDeviation,
                    RatingI = ratingI,
                    RatingJ = ratingJ
                });
                
                ResultModel.Add(new DeviationUpdateModel
                {
                    UpdatePair = FlipPair(updatePair),
                    NewDeviation = FlipResult(newCurrentDeviation),
                    OldDeviation = currentDeviation,
                    RatingI = ratingI,
                    RatingJ = ratingJ
                });
            }

            watch.Stop();
            double elapsed = watch.Elapsed.TotalSeconds;
            System.Console.WriteLine();
        }

        private static double FlipResult(double deviation)
        {
            return (deviation < 0) ? Math.Abs(deviation) : deviation*-1;
        }

        private static Tuple<int,int> FlipPair(Tuple<int, int> pair)
        {
            return new Tuple<int, int>(pair.Item2, pair.Item1);
        }
    }
}