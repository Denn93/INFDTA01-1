using System.Linq;
using System.Web.Mvc;
using BrianDennis.INFDTA01.Opdracht1.Models;
using BrianDennis.INFDTA01.Opdracht1.Models.BijlageC;
using BrianDennis.INFDTA01.Opdracht1.Services;
using BrianDennis.INFDTA01.Opdracht1.Services.BijlageC;

namespace BrianDennis.INFDTA01.Opdracht1.Controllers
{
    public class MatrixController : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViewModel {Data = UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix).ToDictionary(m=>m.Key, m=>m.Value)});
        }

        public ActionResult Mean()
        {
            return View(new MeanViewModel {ResultList = Deliverables.MovieRatingMeans()});
        }

        public ActionResult PercentOfFours()
        {
            return View(new MeanViewModel {ResultList = Deliverables.PercentOfFours()});
        }
    }
}