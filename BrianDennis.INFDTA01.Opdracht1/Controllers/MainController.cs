using System.Collections.Generic;
using System.Linq;
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
                        UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView()).Calculate(targetUser ?? int.Parse(Configuration.Targets(RetrieveView())["DefaultTarget"]))
            };

            return View(model);
        }

        public ActionResult Pearson(int? targetUser)
        {
            PlainViewModel model = new PlainViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson, UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView())
                        .Calculate(targetUser ?? int.Parse(Configuration.Targets(RetrieveView())["DefaultTarget"]))
            };

            return View(model);
        }

        public ActionResult Cosine(int? targetUser)
        {
            PlainViewModel model = new PlainViewModel
            {
                Data =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Cosine, UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView())
                        .Calculate(targetUser ?? int.Parse(Configuration.Targets(RetrieveView())["DefaultTarget"]))
            };

            return View(model);
        }

        public ActionResult Predictive(int? targetUser)
        {
            PredictiveRatingViewModel model = new PredictiveRatingViewModel();
            List<AlgorithmResultListItem> pearsonListData = new List<AlgorithmResultListItem>();

            if (RetrieveView().Equals("MovieLens"))
            {
                pearsonListData =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson,
                        UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView())
                        .Calculate(targetUser ?? int.Parse(Configuration.Targets(RetrieveView())["DefaultTarget"]));
            }
            else
            {
                pearsonListData =
                    AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson,
                        UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView())
                        .Calculate(targetUser ?? int.Parse(Configuration.Targets(RetrieveView())["DefaultTarget"]));
            }

            PredictingRatingAlgorithm predictive = (PredictingRatingAlgorithm)
                AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Predictive, UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView());

            predictive.PearsonListData = pearsonListData;
            predictive.Calculate(targetUser ?? int.Parse(Configuration.Targets(RetrieveView())["DefaultTarget"]));

            model.Data = predictive.PredictiveRatings;

            if (RetrieveView().Equals("MovieLens"))
            {
                model.Data = model.Data.OrderByDescending(m => m.Value).Take(8).ToDictionary(m => m.Key, m => m.Value);

            }

            return View(model);
        }

        public ActionResult Deviation()
        {
            ItemItemDeviationAlgorithm algorithm = (ItemItemDeviationAlgorithm) AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Deviation,
                UserItemDataSetFactory.GetDatasetByString(RetrieveView()), RetrieveView());

            algorithm.Calculate();
            
            return View(algorithm.DeviationResult);
        }

        #endregion

        #region Additional Methods

        private string RetrieveView()
        {
            return ControllerContext.RouteData.Values["view"].ToString();
        }

        #endregion

        #region Protected Methods 

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.TargetUser = Configuration.Targets(RetrieveView())["DefaultTarget"];
            filterContext.Controller.ViewBag.NearestNeighbours = Configuration.Targets(RetrieveView())["NearestNeighbours"];
            filterContext.Controller.ViewBag.ThresHold = Configuration.Targets(RetrieveView())["InitialThreshold"];

            base.OnActionExecuted(filterContext);
        }

        #endregion
    }
}   