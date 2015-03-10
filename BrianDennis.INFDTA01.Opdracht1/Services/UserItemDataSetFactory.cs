using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public static class UserItemDataSetFactory
    {
        /// <summary>
        /// Enum available datasets
        /// </summary>
        public enum DataSets
        {
            UserItem,
            UserItemEdited,
            MovieLens,
            NotFound
        }

        #region Factory methods

        /// <summary>
        /// Factory method creating the Dataset
        /// </summary>
        /// <param name="dataSet">DataSet enum item</param>
        /// <returns>Created Dataset</returns>
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

        #endregion

        #region Private Helper methods

        /// <summary>
        /// This private methods loads a Dataset based on filepath and added split character
        /// </summary>
        /// <param name="filePath">File path to Dataset</param>
        /// <param name="split">Split character</param>
        /// <returns>Loaded dataset into Dictionary<int, Dictionary<int, float>></returns>
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

        /// <summary>
        /// This private method processes the line of the loaded Dataset.
        /// </summary>
        /// <param name="line">Input line</param>
        /// <param name="split">Split line parameter</param>
        /// <param name="dataSet">Resulting dataset</param>
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

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// This method gets the Dataset by String value of the Dataset enum item
        /// </summary>
        /// <param name="value">DataSet value String</param>
        /// <returns>Enum DataSets</returns>
        public static DataSets GetDatasetByString(string value)
        {
            foreach (DataSets dataSet in Enum.GetValues(typeof(DataSets)).Cast<DataSets>().Where(dataSet => dataSet.ToString().ToLower().Equals(value.ToLower())))
                return dataSet;

            return DataSets.NotFound;
        }

        #endregion

        #region Properties

        public static SortedDictionary<int, Dictionary<int, float>> UserItemDataSet { get; set; }
        public static SortedDictionary<int, Dictionary<int, float>> UserItemEditedDataSet { get; set; }
        public static SortedDictionary<int, Dictionary<int, float>> MovieLensDataSet { get; set; }

        #endregion

    }
}