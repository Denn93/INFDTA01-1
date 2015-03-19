using System.Web.Mvc;
using BrianDennis.INFDTA01.Opdracht1.Models;
using BrianDennis.INFDTA01.Opdracht1.Services;

namespace BrianDennis.INFDTA01.Opdracht1.Controllers
{
    public class MatrixController : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViewModel {Data = UserItemDataSetFactory.Build(UserItemDataSetFactory.DataSets.Matrix)});
        }
    }
}