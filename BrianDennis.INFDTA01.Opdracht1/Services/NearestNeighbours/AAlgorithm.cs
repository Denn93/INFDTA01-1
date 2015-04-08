using System;
using System.Collections.Generic;
using System.Linq;
using BrianDennis.INFDTA01.Opdracht1.Models;

namespace BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours
{
    public abstract class AAlgorithm
    {
        protected double ThresHold { get; set; }

        public virtual void Calculate()
        {
            throw new NotImplementedException();
        }

        public virtual List<AlgorithmResultListItem> Calculate(int targetUser)
        {
            throw new NotImplementedException();
        }

        public virtual void Calculate(int targetUser, int? targetItem)
        {
            throw new NotImplementedException();
        }

        public virtual void Calculate(int targetUser, int article, float ratingI)
        {
            throw new NotImplementedException();
        }

        protected SortedDictionary<int, UserPreference> DataSet;

        protected AAlgorithm(SortedDictionary<int, UserPreference> dataSet, string view)
        {
            View = view;
            DataSet = dataSet;
        }

        /// <summary>
        /// This method determines if a item with the calculated 
        /// similarity can be added to the list. 
        /// </summary>
        /// <param name="list">The resultList</param>
        /// <param name="item">Item to be added</param>
        /// <param name="similarity">item similarity</param>
        protected void ResultAdd(List<AlgorithmResultListItem> list, AlgorithmResultListItem item, double similarity)
        {
            if (similarity > ThresHold)
            {
                if (list.Count < int.Parse(Configuration.Targets(View)["NearestNeighbours"]))
                    list.Add(item);
                else
                {
                    AlgorithmResultListItem lowest =
                        list.Aggregate((item1, item2) => item1.Similarity < item2.Similarity ? item1 : item2);

                    if (lowest.Similarity < similarity)
                    {
                        list.Remove(lowest);
                        list.Add(item);

                        AlgorithmResultListItem newLowest =
                        list.Aggregate((item1, item2) => item1.Similarity < item2.Similarity ? item1 : item2);

                        ThresHold = newLowest.Similarity;
                    }
                }
            }
        }

        protected string View { get; private set; }
    }
}