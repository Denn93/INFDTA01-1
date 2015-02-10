using System.Collections.Generic;
using System.IO;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services
{
    public class UserPreferenceService
    {
        public SortedDictionary<int, List<UserPreferenceModel>> Load()
        {
            string line;
            StreamReader reader = new StreamReader(@"c:\users\dennis\documents\visual studio 2013\Projects\BrianDennis.INFDTA01\BrianDennis.INFDTA01.Opdracht1\userItem.csv");

            SortedDictionary<int, List<UserPreferenceModel>> dataSet = new SortedDictionary<int, List<UserPreferenceModel>>();

            while ((line = reader.ReadLine()) != null)
            {
                string[] row = line.Split(',');

                int userId = int.Parse(row[0]);
                int articleId = int.Parse(row[1]);
                float rating = float.Parse(row[2]);

                if (!dataSet.ContainsKey(userId))
                {
                    List<UserPreferenceModel> content = new List<UserPreferenceModel>() ;
                    content.Add(new UserPreferenceModel {ArticleId = articleId, Rating = rating});
                    dataSet.Add(userId, content);
                }
                else
                {
                    List<UserPreferenceModel> content = dataSet[userId];
                    content.Add(new UserPreferenceModel {ArticleId = articleId, Rating = rating});
                    //dataSet[userId] = content;
                }
            }

;
            return dataSet;
        }
    }
}