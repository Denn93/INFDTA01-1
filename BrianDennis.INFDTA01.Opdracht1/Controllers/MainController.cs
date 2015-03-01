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
            return View(new IndexViewModel {Data = UserPreferenceService.DataSet});
        }

        public ActionResult Euclidean()
        {
            EuclideanViewModel model = new EuclideanViewModel
            {
                Data = AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Euclidean).Calculate(7)
            };

            return View(model);
        }

        public ActionResult Pearson()
        {
            PearsonViewModel model = new PearsonViewModel
            {
                Data = AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Pearson).Calculate(7)
            };

            return View(model);
        }

        public ActionResult Cosine()
        {
            CosineViewModel model = new CosineViewModel
            {
                Data = AlgorithmFactory.Build(AlgorithmFactory.Algorithm.Cosine).Calculate(7)
            };

            return View(model);
        }
    }
}