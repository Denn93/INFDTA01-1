using System.Collections.Generic;
using System.Web.Mvc;
using BrianDennis.INFDTA01.Opdracht1.Models;
using BrianDennis.INFDTA01.Opdracht1.Services;

namespace BrianDennis.INFDTA01.Opdracht1.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            UserPreferenceService service = new UserPreferenceService();
            SortedDictionary<int, List<UserPreferenceModel>> data = service.Load();

            IndexViewModel model = new IndexViewModel {Data = data};

            return View(model);
        }
    }
}