﻿using System.Collections.Generic;
using System.Web.Mvc;
using BrianDennis.INFDTA01.Opdracht1.Models;
using BrianDennis.INFDTA01.Opdracht1.Services;
using BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours;

namespace BrianDennis.INFDTA01.Opdracht1.Controllers
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return
                View(new IndexViewModel {Data = UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.UserItem)});
        }

        public ActionResult Euclidean()
        {
            EuclideanViewModel model = new EuclideanViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Euclidean,
                        UserItemDataSetFactory.DataSets.UserItem).Calculate(7)
            };

            return View(model);
        }

        public ActionResult Pearson()
        {
            PearsonViewModel model = new PearsonViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.DataSets.UserItem)
                        .Calculate(7)
            };

            return View(model);
        }

        public ActionResult Cosine()
        {
            CosineViewModel model = new CosineViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Cosine, UserItemDataSetFactory.DataSets.UserItem)
                        .Calculate(7)
            };

            return View(model);
        }

        public ActionResult Predictive()
        {
            PredictiveRatingViewModel model = new PredictiveRatingViewModel();

            List<AlgorithmResultListItem> pearsonListData =
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.DataSets.UserItem)
                    .Calculate(7);

            PredictingRatingAlgorithm predictive = (PredictingRatingAlgorithm)
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Predictive, UserItemDataSetFactory.DataSets.UserItem);

            predictive.PearsonListData = pearsonListData;
            predictive.Calculate(7);

            model.Data = predictive.PredictiveRatings;

            return View(model);
        }
    }
}