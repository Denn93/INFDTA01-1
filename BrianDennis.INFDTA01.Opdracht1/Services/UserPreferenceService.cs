using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public class UserPreferenceService
    {
        public SortedDictionary<int, List<UserPreferenceModel>> Load()
        {
            SortedDictionary<int, List<UserPreferenceModel>> dataSet = new SortedDictionary<int, List<UserPreferenceModel>>();

            var lines = File.ReadAllLines(Configuration.FilePath);

            Parallel.ForEach(lines, line =>
            {
                lock (dataSet)
                {
                    Process(line, dataSet);
                }
            });

            return dataSet;
        }

        private static void Process(string line, SortedDictionary<int, List<UserPreferenceModel>> dataSet)
        {
            string[] row = line.Split(',');

            int userId = int.Parse(row[0]);
            int articleId = int.Parse(row[1]);
            float rating = float.Parse(row[2]);

            if (!dataSet.ContainsKey(userId))
            {
                List<UserPreferenceModel> content = new List<UserPreferenceModel> { new UserPreferenceModel { ArticleId = articleId, Rating = rating } };
                dataSet.Add(userId, content);
            }
            else
            {
                List<UserPreferenceModel> content = dataSet[userId];
                content.Add(new UserPreferenceModel { ArticleId = articleId, Rating = rating });
            }
        }
    }
}