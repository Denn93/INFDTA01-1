using System;
using System.Collections.Generic;
using System.Diagnostics;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemSlopeUpdate : AAlgorithm
    {
        private Tuple<int, int> _pair;
        private int _ratingI;
        private int _ratingJ;

        public ItemItemSlopeUpdate(SortedDictionary<int, UserPreference> dataSet, string view) 
            : base(dataSet, view)
        {}

        public DeviationUpdateModel ResultModel{ get; set; }
        
        public override void Calculate(int targetUser, int? targetItem)
        {
            Stopwatch watch = new Stopwatch();
            Dictionary<Tuple<int, int>, Tuple<double, int>> deviations = ItemItemDeviationAlgorithm.GetDeviationResult(View);

            double currentDeviation = deviations[_pair].Item1;

            double newCurrentDeviation = ((deviations[_pair].Item1 * deviations[_pair].Item2) + (_ratingI - _ratingJ)) /
                                         deviations[_pair].Item2 + 1;

            deviations[_pair] = new Tuple<double, int>(newCurrentDeviation, deviations[_pair].Item2 + 1);

            ResultModel = new DeviationUpdateModel
            {
                UpdatePair = _pair,
                NewDeviation = newCurrentDeviation,
                OldDeviation = currentDeviation,
                RatingI = _ratingI,
                RatingJ = _ratingJ
            };

            watch.Stop();
            double elapsed = watch.Elapsed.TotalSeconds;
            System.Console.WriteLine();
        }

        public void PreparePairUpdate(int i, int j, int ratingI, int ratingJ)
        {
            _pair = new Tuple<int, int>(i, j);
            _ratingI = ratingI;
            _ratingJ = ratingJ;
        }
    }
}