using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.BijlageC
{
    public class Deliverables
    {
        public static Dictionary<int, float> MovieRatingMeans()
        {
            SortedDictionary<int, UserPreference> dataSet =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix);

            IEnumerable<KeyValuePair<int, float>> movieLists =
                dataSet.SelectMany(m => m.Value.Preferences.Select(i => i));

            return
                movieLists.GroupBy(m => m.Key)
                    .Select(grp => 
                        new {Movie = grp.Key, Mean = (grp.Select(m => m.Value).Sum()/grp.Count())})
                    .OrderByDescending(m => m.Mean)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.Mean);
        }

        public static Dictionary<int, float> PercentOfFours()
        {
            SortedDictionary<int, UserPreference> dataSet =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix);

            IEnumerable<KeyValuePair<int, float>> movieLists =
                dataSet.SelectMany(m => m.Value.Preferences.Select(i => i));

            return
                movieLists.GroupBy(m => m.Key)
                    .Select(
                        grp =>
                            new
                            {
                                Movie = grp.Key,
                                PercentOfFours = ((float) grp.Count(i => i.Value >= 4)/(float) grp.Count())*100
                            })
                    .OrderByDescending(m => m.PercentOfFours)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.PercentOfFours);
        }
    }
}