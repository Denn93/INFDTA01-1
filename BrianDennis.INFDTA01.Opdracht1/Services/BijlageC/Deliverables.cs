using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.BijlageC
{
    public class Deliverables
    {
        public static Dictionary<string, float> MovieRatingMeans()
        {
            SortedDictionary<int, List<UserPreference>> dataSet =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix);

            List<UserPreference> movieLists = dataSet.SelectMany(keyValuePair => keyValuePair.Value).ToList();

            return
                movieLists.GroupBy(m => m.Description)
                    .Select(grp => new {Key = grp.Key, Ratings = grp.Select(m => m.Rating).ToList()})
                    .ToList()
                    .Select(m => new {Movie = m.Key, Mean = (m.Ratings.Sum()/m.Ratings.Count)})
                    .OrderByDescending(m => m.Mean)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.Mean);
        }

        public static Dictionary<string, float> PercentOfFours()
        {
            SortedDictionary<int, List<UserPreference>> dataSet =
                UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix);

            List<UserPreference> movieLists = dataSet.SelectMany(keyValuePair => keyValuePair.Value).ToList();

            return
                movieLists.GroupBy(m => m.Description)
                    .Select(grp => new {Key = grp.Key, Ratings = grp.Select(m => m.Rating).ToList()})
                    .ToList()
                    .Select(m => new {Movie = m.Key, PercentOfFours = ((float)m.Ratings.Count(i=>i >= 4) / (float)m.Ratings.Count) * 100})
                    .OrderByDescending(m => m.PercentOfFours)
                    .Take(5)
                    .ToDictionary(m => m.Movie, m => m.PercentOfFours);
        }

    }
}