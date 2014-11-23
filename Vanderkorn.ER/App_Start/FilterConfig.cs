using System.Web;
using System.Web.Mvc;

namespace Vanderkorn.ER
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
          //  filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }
    }
}
