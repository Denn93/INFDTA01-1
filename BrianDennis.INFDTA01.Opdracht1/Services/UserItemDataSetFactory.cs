using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public static class UserItemDataSetFactory
    {
        public enum DataSets
        {
            UserItem,
            UserItemEdited,
            MovieLens,
            NotFound
        }

        public static SortedDictionary<int, Dictionary<int, float>> UserItemDataSet { get; set; }
        public static SortedDictionary<int, Dictionary<int, float>> UserItemEditedDataSet { get; set; }
        public static SortedDictionary<int, Dictionary<int, float>> MovieLensDataSet { get; set; }
   
        public static SortedDictionary<int, Dictionary<int, float>> Build(DataSets dataSet)
        {
            switch (dataSet)
            {
                case DataSets.UserItem:
                    return UserItemDataSet ?? (UserItemDataSet = LoadingConstruct(Configuration.UserItemCsvPath, ','));
                case DataSets.UserItemEdited:
                    return UserItemEditedDataSet ??
                           (UserItemEditedDataSet = LoadingConstruct(Configuration.UserItemEditedCsvPath, ','));
                    case DataSets.MovieLens:
                        return MovieLensDataSet ??
                           (MovieLensDataSet = LoadingConstruct(Configuration.MovieLensData, '\t'));
                case DataSets.NotFound:
                    return null;
            }

            return null;
        }

        private static SortedDictionary<int, Dictionary<int, float>> LoadingConstruct(string filePath, char split)
        {
            SortedDictionary<int, Dictionary<int, float>> dataSet = new SortedDictionary<int, Dictionary<int, float>>();

            var lines = File.ReadAllLines(filePath);

            Parallel.ForEach(lines, line =>
            {
                lock (dataSet)
                {
                    ProcessConstruct(line, split, dataSet);
                }
            });

            return dataSet;
        }

        private static void ProcessConstruct(string line, char split, IDictionary<int, Dictionary<int, float>> dataSet)
        {
            string[] row = line.Split(split);

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

        public static DataSets GetDatasetByString(string value)
        {
            foreach (DataSets dataSet in Enum.GetValues(typeof(DataSets)).Cast<DataSets>().Where(dataSet => dataSet.ToString().ToLower().Equals(value.ToLower())))
                return dataSet;

            return DataSets.NotFound;
        }
    }
}