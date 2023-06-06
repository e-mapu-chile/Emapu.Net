using System.Web.Mvc;

namespace SSoft.IoT.MVC.Areas.CultivoApi
{
    public class CultivoApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CultivoApi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CultivoApi_default",
                "CultivoApi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
