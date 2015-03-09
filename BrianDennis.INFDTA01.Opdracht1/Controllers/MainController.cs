using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using BrianDennis.INFDTA01.Opdracht1.Models;
using BrianDennis.INFDTA01.Opdracht1.Services;
using BrianDennis.INFDTA01.Opdracht1.Services.NearestNeighbours;

namespace BrianDennis.INFDTA01.Opdracht1.Controllers
{
    public class MainController : Controller
    {
        #region GET Requests

        public ActionResult Index()
        {
            return
                View(new IndexViewModel { Data = UserItemDataSetFactory.Build(UserItemDataSetFactory.GetDatasetByString(RetrieveView())) });
        }

        public ActionResult Euclidean()
        {
            EuclideanViewModel model = new EuclideanViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Euclidean,
                        UserItemDataSetFactory.GetDatasetByString(RetrieveView())).Calculate(7)
            };

            return View(model);
        }

        public ActionResult Pearson()
        {
            PearsonViewModel model = new PearsonViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.GetDatasetByString(RetrieveView()))
                        .Calculate(7)
            };

            return View(model);
        }

        public ActionResult Cosine()
        {
            CosineViewModel model = new CosineViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Cosine, UserItemDataSetFactory.GetDatasetByString(RetrieveView()))
                        .Calculate(7)
            };

            return View(model);
        }

        public ActionResult Predictive()
        {
            PredictiveRatingViewModel model = new PredictiveRatingViewModel();

            List<AlgorithmResultListItem> pearsonListData =
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.GetDatasetByString(RetrieveView()))
                    .Calculate(7);

            PredictingRatingAlgorithm predictive = (PredictingRatingAlgorithm)
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Predictive, UserItemDataSetFactory.GetDatasetByString(RetrieveView()));

            predictive.PearsonListData = pearsonListData;
            predictive.Calculate(7);

            model.Data = predictive.PredictiveRatings;

            return View(model);
        }

        #endregion

        #region Additional Methods

        private string RetrieveView()
        {
            return ControllerContext.RouteData.Values["view"].ToString();
        }

        #endregion
    }
}