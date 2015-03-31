using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemDeviationAlgorithm : AAlgorithm
    {
        public List<DeviationResultItem> DeviationResult{ get; set; }
        public Stack<DeviationResultItem> NewResult { get; set; }
        private List<List<UserPreference>> ReFormatDataSet;
        private int ReformatCount;


        public ItemItemDeviationAlgorithm(SortedDictionary<int, List<UserPreference>> dataSet, string view)
            : base(dataSet, view)
        {
            NewResult = new Stack<DeviationResultItem>();
            DeviationResult = new List<DeviationResultItem>();
            ReFormatDataSet = DataSet.Select(m => m.Value).ToList();
            ReformatCount = ReFormatDataSet.Count;
        }


        public override void Calculate()
        {
            IEnumerable<Tuple<int, int>> pairs = CreatePairs();
            Parallel.ForEach(pairs, pair =>
                Deviation(pair.Item1, pair.Item2)
                );
        }

        private IEnumerable<Tuple<int, int>> CreatePairs()
        {
            List<int> items =
                DataSet.Values.SelectMany(m => m.Select(i => i.MovieId))
                    .GroupBy(m => m)
                    .Select(m => m.First())
                    .ToList();

            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i; j < items.Count; j++)
                {
                    if (i == j) continue;       
 
                    //Deviation(i, j);
                    pairs.Add(new Tuple<int, int>(items[i], items[j]));
                }
            }

            return pairs.AsEnumerable();
        }

        private void Deviation(int i, int j)
        {
            double currentDeviation = 0.0;
            int userCount = 0;

            /*for (int k = 0; k < ReformatCount; k++)
            {
                int l = (ReformatCount - 1) - k;

                if (k == l || l < k)
                    break;

                if (ReFormatDataSet[k].Exists(m => m.MovieId == i))
                {
                    if (ReFormatDataSet[k].Exists(m => m.MovieId == j))
                    {
                        float ratingItem1 = ReFormatDataSet[k].Single(m => m.MovieId == i).Rating;
                        float ratingItem2 = ReFormatDataSet[k].Single(m => m.MovieId == j).Rating;

                        currentDeviation += ratingItem1 - ratingItem2;
                        userCount++;
                    }
                }

                if (ReFormatDataSet[l].Exists(m => m.MovieId == i))
                {
                    if (ReFormatDataSet[l].Exists(m => m.MovieId == j))
                    {
                        float ratingItem1 = ReFormatDataSet[l].Single(m => m.MovieId == i).Rating;
                        float ratingItem2 = ReFormatDataSet[l].Single(m => m.MovieId == j).Rating;

                        currentDeviation += ratingItem1 - ratingItem2;
                        userCount++;
                    }
                }
            }*/
/*            Parallel.ForEach(DataSet, dataRow =>
            
                if (dataRow.Value.Exists(m => m.MovieId == i && m.MovieId == j))
                {
                    float ratingItem1 = dataRow.Value.Single(m => m.MovieId == i).Rating;
                    float ratingItem2 = dataRow.Value.Single(m => m.MovieId == j).Rating;

                    currentDeviation += ratingItem1 - ratingItem2;
                    userCount++;
                }
           );*/

            foreach (KeyValuePair<int, List<UserPreference>> keyValuePair in DataSet)
            {
                if (keyValuePair.Value.Exists(m=>m.MovieId == i) && keyValuePair.Value.Exists(m=>m.MovieId == j))
                {
                    float ratingItem1 = keyValuePair.Value.Single(m => m.MovieId == i).Rating;
                    float ratingItem2 = keyValuePair.Value.Single(m => m.MovieId == j).Rating;

                    currentDeviation += ratingItem1 - ratingItem2;
                    userCount++;
                }
            }

            currentDeviation = currentDeviation/userCount;
/*

            NewResult.Push(DeviationResultItem.Build(new Tuple<int, int, double, int>(i, j, currentDeviation,
                    userCount)));

            NewResult.Push(DeviationResultItem.Build(new Tuple<int, int, double, int>(j, i,
                    (currentDeviation < 0) ? Math.Abs(currentDeviation) : currentDeviation * -1,
                    userCount)));
*/

            DeviationResult.Add(DeviationResultItem.Build(new Tuple<int, int, double, int>(i, j, currentDeviation,
                userCount))
                );

            DeviationResult.Add(
                DeviationResultItem.Build(new Tuple<int, int, double, int>(j, i,
                    (currentDeviation < 0) ? Math.Abs(currentDeviation) : currentDeviation*-1,
                    userCount)));
        }
    }
}