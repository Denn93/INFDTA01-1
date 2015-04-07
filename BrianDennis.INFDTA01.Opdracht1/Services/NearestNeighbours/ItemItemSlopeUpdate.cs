using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemSlopeUpdate : AAlgorithm
    {
        private List<Tuple<int, int>> _pairs;
        private int _i;
        private float _ratingI;

        public ItemItemSlopeUpdate(SortedDictionary<int, UserPreference> dataSet, string view) 
            : base(dataSet, view)
        {}

        public List<DeviationUpdateModel> ResultModel{ get; set; }
        
        public override void Calculate(int targetUser, int? targetItem)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            ResultModel = new List<DeviationUpdateModel>();
            Dictionary<Tuple<int, int>, Tuple<double, int>> deviations = ItemItemDeviationAlgorithm.GetDeviationResult(View);

            SortedDictionary<int, UserPreference> dataset =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.userItemCsv);
            dataset[targetUser].Preferences.Add(_i, _ratingI);

            List<Tuple<int, int>> updatePairs =
                ItemItemDeviationAlgorithm.GetPairList(View).Where(m => m.Item1 == _i || m.Item2 == _i).ToList();

            foreach (Tuple<int, int> updatePair in updatePairs)
            {
                if (!dataset[targetUser].Preferences.ContainsKey(updatePair.Item1) || !dataset[targetUser].Preferences.ContainsKey(updatePair.Item2))
                    continue;


                float ratingJ = (updatePair.Item1 != _i)
                    ? dataset[targetUser].Preferences[updatePair.Item1]
                    : dataset[targetUser].Preferences[updatePair.Item2];

                double currentDeviation = deviations[updatePair].Item1;
                double newCurrentDeviation = ((deviations[updatePair].Item1*deviations[updatePair].Item2) +
                                              (_ratingI - ratingJ)) /
                                             (deviations[updatePair].Item2 + 1);

                deviations[updatePair] = new Tuple<double, int>(newCurrentDeviation, deviations[updatePair].Item2 + 1);

                deviations[FlipPair(updatePair)] = new Tuple<double, int>(FlipResult(newCurrentDeviation), deviations[FlipPair(updatePair)].Item2 + 1);

                ResultModel.Add(new DeviationUpdateModel
                {
                    UpdatePair = updatePair,
                    NewDeviation = newCurrentDeviation,
                    OldDeviation = currentDeviation,
                    RatingI = _ratingI,
                    RatingJ = ratingJ
                });

                ResultModel.Add(new DeviationUpdateModel
                {
                    UpdatePair = FlipPair(updatePair),
                    NewDeviation = FlipResult(newCurrentDeviation),
                    OldDeviation = currentDeviation,
                    RatingI = _ratingI,
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

        public void AddRating(int i, float ratingI)
        {
            _i = i;
            _ratingI = ratingI;
        }
    }
}