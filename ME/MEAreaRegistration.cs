using System.Web.Mvc;

namespace HRRcp.Areas.ME
{
    public class MEAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ME";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
            "ME_default",
            "ME/{controller}/{action}/{id}",
            new { action = "Index", id = UrlParameter.Optional }
            ).DataTokens.Add("area2", "ME");
        }
    }
}