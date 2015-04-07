using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public class ItemItemDeviationAlgorithm : AAlgorithm
    {
        public ItemItemDeviationAlgorithm(SortedDictionary<int, UserPreference> dataSet, string view)
            : base(dataSet, view)
        {}

        public static Dictionary<Tuple<int, int>, Tuple<double, int>> DeviationResultUserItem  { get; set; }
        public static Dictionary<Tuple<int, int>, Tuple<double, int>> DeviationResultUserItemEdited  { get; set; }
        public static Dictionary<Tuple<int, int>, Tuple<double, int>> DeviationResultMovieLens  { get; set; }

        public static List<int> ItemListMovieLens { get; set; } 
        public static List<int> ItemListUserItem { get; set; } 
        public static List<int> ItemListUserItemEdited { get; set; } 

        public static List<Tuple<int, int>> PairListMovieLens { get; set; }
        public static List<Tuple<int, int>> PairListUserItem { get; set; }
        public static List<Tuple<int, int>> PairListUserItemEdited { get; set; } 

        public override void Calculate()
        {
            IEnumerable<Tuple<int, int>> pairs = CreatePairs();
            Dictionary<Tuple<int, int>, Tuple<int, double>> devList = new Dictionary<Tuple<int, int>, Tuple<int, double>>();

            Stopwatch watch = new Stopwatch();   
            watch.Start();
            int skipped = 0;

            Parallel.ForEach(DataSet, user =>
            {
                foreach (Tuple<int, int> pair in pairs)
                {
                    if (user.Value.Preferences.ContainsKey(pair.Item1) && user.Value.Preferences.ContainsKey(pair.Item2))
                    {
                        lock (devList)
                        {
                            if (devList.ContainsKey(pair))
                                devList[pair] = new Tuple<int, double>(devList[pair].Item1 + 1,
                                    devList[pair].Item2 +
                                    (user.Value.Preferences[pair.Item1] - user.Value.Preferences[pair.Item2]));
                            else
                                devList.Add(pair,
                                    new Tuple<int, double>(1,
                                        user.Value.Preferences[pair.Item1] - user.Value.Preferences[pair.Item2]));
                        }
                    }
                }
            });

            Dictionary<Tuple<int, int>, Tuple<double, int>> result = new Dictionary<Tuple<int, int>, Tuple<double, int>>();
            foreach (KeyValuePair<Tuple<int, int>, Tuple<int, double>> item in devList)
            {
                double currentDeviation = item.Value.Item2/item.Value.Item1;

                result.Add(new Tuple<int, int>(item.Key.Item1, item.Key.Item2),
                    new Tuple<double, int>(currentDeviation, item.Value.Item1));

                result.Add(new Tuple<int, int>(item.Key.Item2, item.Key.Item1),
                    new Tuple<double, int>((currentDeviation < 0) ? Math.Abs(currentDeviation) : currentDeviation*-1,
                        item.Value.Item1));
            }

            switch (View)
            {
                case "MovieLens":
                    DeviationResultMovieLens = result;
                    break;
                case "userItemCsv":
                    DeviationResultUserItem = result;
                    break;
                case "userItemEditedCsv":
                    DeviationResultUserItemEdited = result;
                    break;
            }


            devList.Clear();

            watch.Stop();
            double elapsed = watch.Elapsed.TotalSeconds;
            System.Console.WriteLine();
        }

        public IEnumerable<Tuple<int, int>> CreatePairs()
        {
            var itemList = DataSet.Values.SelectMany(m => m.Preferences.Select(i => i.Key)).Distinct().ToList();

            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
            for (int i = 0; i < itemList.Count; i++)
            {
                for (int j = i; j < itemList.Count; j++)
                {
                    if (i == j) continue;
                    pairs.Add(new Tuple<int, int>(itemList[i], itemList[j]));
                }
            }

            switch (View)
            {
                case "MovieLens":
                    PairListMovieLens = pairs;
                    ItemListMovieLens = itemList;
                    break;
                case "userItemCsv":
                    PairListUserItem = pairs;
                    ItemListUserItem = itemList;
                    break;
                case "userItemEditedCsv":
                    PairListUserItemEdited = pairs;
                    ItemListUserItemEdited = itemList;
                    break;
            }


            return pairs.AsEnumerable();
        }

        public static Dictionary<Tuple<int, int>, Tuple<double, int>> GetDeviationResult(string view)
        {
            switch (view)
            {
                case "MovieLens":
                    return DeviationResultMovieLens;
                case "userItemCsv":
                    return DeviationResultUserItem;
                case "userItemEditedCsv":
                    return DeviationResultUserItemEdited;
                default:
                    return null;
            }
        }

        public static List<int> GetItemList(string view)
        {
            switch (view)
            {
                case "MovieLens":
                    return ItemListMovieLens;
                case "userItemCsv":
                    return ItemListUserItem;
                case "userItemEditedCsv":
                    return ItemListUserItemEdited;
                default:
                    return null;
            }
        }

        public static List<Tuple<int, int>> GetPairList(string view)
        {
            switch (view)
            {
                case "MovieLens":
                    return PairListMovieLens;
                case "userItemCsv":
                    return PairListUserItem;
                case "userItemEditedCsv":
                    return PairListUserItemEdited;
                default:
                    return null;
            }
        }
    }
}