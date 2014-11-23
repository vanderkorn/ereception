namespace Vanderkorn.ER.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ReceptionController : Controller
    {
        // GET: ElectronicReception
        public ActionResult Index()
        {
            return this.View();
        }
    }
}