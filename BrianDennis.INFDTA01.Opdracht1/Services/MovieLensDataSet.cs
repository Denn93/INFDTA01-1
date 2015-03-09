using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public class MovieLensDataSet
    {
        public SortedDictionary<int, Dictionary<int, float>> DataSet { get; set; }


        public static SortedDictionary<int, Dictionary<int, float>> LoadingConstruct(string filePath)
        {
            SortedDictionary<int, Dictionary<int, float>> dataSet = new SortedDictionary<int, Dictionary<int, float>>();

            var lines = File.ReadAllLines(Configuration.MovieLensData);

            Parallel.ForEach(lines, line =>
            {
                lock (dataSet)
                {
                    ProcessConstruct(line, dataSet);
                }
            });

            return dataSet;
        }

        private static void ProcessConstruct(string line, IDictionary<int, Dictionary<int, float>> dataSet)
        {
            string[] row = line.Split('\t');

            int userId = int.Parse(row[0]);
            int articleId = int.Parse(row[1]);
            float rating = float.Parse(row[2]);

            if (!dataSet.ContainsKey(userId))
            {
                Dictionary<int, float> content = new Dictionary<int, float> { { articleId, rating } };
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