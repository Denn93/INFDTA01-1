using System.Collections.Generic;
using System.Web.Mvc;
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
            return RetrieveView().Equals("MovieLens") ? View("Error") : View(new IndexViewModel { Data = UserItemDataSetFactory.Build(UserItemDataSetFactory.GetDatasetByString(RetrieveView())) });
        }

        public ActionResult Euclidean(int? targetUser)
        {
            PlainViewModel model = new PlainViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Euclidean,
                        UserItemDataSetFactory.GetDatasetByString(RetrieveView())).Calculate(targetUser ?? 7)
            };

            return View(model);
        }

        public ActionResult Pearson(int? targetUser)
        {
            PlainViewModel model = new PlainViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.GetDatasetByString(RetrieveView()))
                        .Calculate(targetUser ?? 7)
            };

            return View(model);
        }

        public ActionResult Cosine(int? targetUser)
        {
            PlainViewModel model = new PlainViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Cosine, UserItemDataSetFactory.GetDatasetByString(RetrieveView()))
                        .Calculate(targetUser ?? 7)
            };

            return View(model);
        }

        public ActionResult Predictive(int? targetUser)
        {
            PredictiveRatingViewModel model = new PredictiveRatingViewModel();

            List<AlgorithmResultListItem> pearsonListData =
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.GetDatasetByString(RetrieveView()))
                    .Calculate(targetUser ?? 7);

            PredictingRatingAlgorithm predictive = (PredictingRatingAlgorithm)
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Predictive, UserItemDataSetFactory.GetDatasetByString(RetrieveView()));

            predictive.PearsonListData = pearsonListData;
            predictive.Calculate(targetUser ?? 7);

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