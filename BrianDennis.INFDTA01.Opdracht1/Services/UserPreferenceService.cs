using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public class UserPreferenceService
    {
        public static SortedDictionary<int, Dictionary<int, float>> DataSet { get; set; }

        public SortedDictionary<int, Dictionary<int, float>> Load()
        {
            SortedDictionary<int, Dictionary<int, float>> dataSet = new SortedDictionary<int, Dictionary<int, float>>();

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

        private static void Process(string line, SortedDictionary<int, Dictionary<int ,float>> dataSet)
        {
            string[] row = line.Split(',');

            int userId = int.Parse(row[0]);
            int articleId = int.Parse(row[1]);
            float rating = float.Parse(row[2]);

            if (!dataSet.ContainsKey(userId))
            {
                Dictionary<int, float> content = new Dictionary<int, float> {{articleId, rating}};
                dataSet.Add(userId, content);
            }
            else
            {
                Dictionary<int, float> content = dataSet[userId];
                content.Add(articleId, rating);
            }
        }
    }
}