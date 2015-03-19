﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public static class UserItemDataSetFactory
    {
        /// <summary>
        /// Enum available datasets
        /// </summary>
        public enum DataSets
        {
            userItemCsv,
            userItemEditedCsv,
            MovieLens,
            Matrix,
            NotFound
        }

        #region Factory methods

        /// <summary>
        /// Factory method creating the Dataset
        /// </summary>
        /// <param name="dataSet">DataSet enum item</param>
        /// <returns>Created Dataset</returns>
        public static SortedDictionary<int, List<UserPreference>> Build(DataSets dataSet)
        {
            switch (dataSet)
            {
                case DataSets.userItemCsv:
                    return UserItemDataSet ??
                           (UserItemDataSet = LoadingConstruct(Configuration.Targets(dataSet.ToString())["Path"], ',', false));
                case DataSets.userItemEditedCsv:
                    return UserItemEditedDataSet ??
                           (UserItemEditedDataSet =
                               LoadingConstruct(Configuration.Targets(dataSet.ToString())["Path"], ',', false));
                    case DataSets.MovieLens:
                    return MovieLensDataSet ??
                           (MovieLensDataSet = LoadingConstruct(Configuration.Targets(dataSet.ToString())["Path"], '\t', false));
                case DataSets.Matrix:
                    return MatrixDataSet ??
                           (MatrixDataSet = LoadingConstruct(Configuration.Targets(dataSet.ToString())["Path"], ',', true));
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
        /// <param name="headers">Headers of file</param>
        /// <returns>Loaded dataset into Dictionary<int, Dictionary<int, float>></returns>
        private static SortedDictionary<int, List<UserPreference>> LoadingConstruct(string filePath, char split, bool headers)
        {
            SortedDictionary<int, List<UserPreference>> dataSet = new SortedDictionary<int, List<UserPreference>>();

            var lines = File.ReadAllLines(filePath);

            string[] headersString = null;

            if (headers)
                headersString = lines.First().Split(split).ToArray(); 


            Parallel.ForEach(lines, line =>
            {
                lock (dataSet)
                {
                    if (!headers)
                        ProcessConstruct(line, split, dataSet);
                    else
                    {
                        ProcessContructHeaderLess(line, split, dataSet, headersString);
                    }
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
        private static void ProcessConstruct(string line, char split, IDictionary<int, List<UserPreference>> dataSet)
        {
            string[] row = line.Split(split);

            int userId = int.Parse(row[0]);
            int articleId = int.Parse(row[1]);
            float rating = float.Parse(row[2]);

            if (!dataSet.ContainsKey(userId))
            {
                List<UserPreference> content = new List<UserPreference>
                {
                    new UserPreference {MovieId = articleId, Rating = rating}
                };

                dataSet.Add(userId, content);
            }
            else
            {
                List<UserPreference> content = dataSet[userId];
                content.Add(new UserPreference {MovieId = articleId, Rating = rating});
            }    
        }

        #endregion

        private static void ProcessContructHeaderLess(string line, char split,
            IDictionary<int, List<UserPreference>> dataSet, string[] headers)
        {
            string[] items = line.Split(split);

            if (!line.StartsWith("User"))
            {
                for (int i = 0; i < items.Count(); i++)
                {
                    if (i == 0)
                        continue;

                    if (items[i].Equals(""))
                        continue;

                    int userId = int.Parse(items[0]);

                    string article = headers[i];
                    
                    if (!dataSet.ContainsKey(userId))
                    {
                        List<UserPreference> content = new List<UserPreference> {new UserPreference {Description = article, Rating = float.Parse(items[i])}};
                        dataSet.Add(userId, content);
                    }
                    else
                    {
                        List<UserPreference> content = dataSet[userId];
                        content.Add(new UserPreference {Description = article, Rating = float.Parse(items[i])});
                    }
                }    
            }
        }

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

        public static SortedDictionary<int, List<UserPreference>> UserItemDataSet { get; set; }
        public static SortedDictionary<int, List<UserPreference>> UserItemEditedDataSet { get; set; }
        public static SortedDictionary<int, List<UserPreference>> MovieLensDataSet { get; set; }
        public static SortedDictionary<int, List<UserPreference>> MatrixDataSet  { get; set; }

        #endregion

    }
}