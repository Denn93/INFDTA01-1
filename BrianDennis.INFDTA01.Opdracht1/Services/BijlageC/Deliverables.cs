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

            IEnumerable<KeyValuePair<int, float>> movieLists = dataSet.SelectMany(m=>m.Value.Preferences.Select(i=>i));

            return
                movieLists.GroupBy(m => m.Key)
                    .Select(grp => new {Movie = grp.Key, Mean = (grp.Select(m => m.Value).Sum()/grp.Count())})
                    .OrderByDescending(m => m.Mean)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.Mean);
/*
                    .Select(m=> new )

                    .Select(grp => new {Key = grp.Key, Ratings = grp.Select(m => m.V).ToList()})
                    .ToList()
                    .Select(m => new {Movie = m.Key, Mean = (m.Ratings.Sum()/m.Ratings.Count)})
                    .OrderByDescending(m => m.Mean)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.Mean);*/
        }

        public static Dictionary<int, float> PercentOfFours()
        {
            SortedDictionary<int, UserPreference> dataSet =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix);

/*
            List<UserPreference> movieLists = dataSet.SelectMany(keyValuePair => keyValuePair.Value).ToList();
*/
            IEnumerable<KeyValuePair<int, float>> movieLists = dataSet.SelectMany(m => m.Value.Preferences.Select(i => i));


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
/*
            return
                movieLists.GroupBy(m => m.Description)
                    .Select(grp => new {Key = grp.Key, Ratings = grp.Select(m => m.Rating).ToList()})
                    .ToList()
                    .Select(m => new {Movie = m.Key, PercentOfFours = ((float)m.Ratings.Count(i=>i >= 4) / (float)m.Ratings.Count) * 100})
                    .OrderByDescending(m => m.PercentOfFours)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.PercentOfFours);*/
        }

    }
}