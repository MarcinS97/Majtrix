using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HRRcp.App_Code
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                     string text, string title, string action,
                                     string controller,
                                     object routeValues = null,
                                     object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

        
    }

    public static class Modals
    {
        public static MvcHtmlString ShowModal(this HtmlHelper htmlHelper, string text, string actionName, string controllerName, string cssClass, object routeValues = null, object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder spanBuilder = new TagBuilder("span");
            spanBuilder.AddCssClass(cssClass);
            spanBuilder.SetInnerText(text);

            TagBuilder aBuilder = new TagBuilder("a");
            //aBuilder.AddCssClass("remove");
            aBuilder.Attributes["data-toggle"] = "modal";
            aBuilder.Attributes["modal"] = urlHelper.Action(actionName, controllerName, routeValues);
       //     aBuilder.Attributes["href"] = "javascript:";
            aBuilder.InnerHtml = spanBuilder.ToString();
            aBuilder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(aBuilder.ToString());
        }

        public static MvcHtmlString ShowModal(this HtmlHelper htmlHelper, string text, string actionName, string cssClass, object routeValues = null, object htmlAttributes = null)
        {
            return ShowModal(htmlHelper, text, actionName, null, cssClass, routeValues, htmlAttributes);
        }


        public static void ShowMessage(dynamic vb, string message)
        {
            vb.___ModalMessage___ = message;
        }
    }


    

   
}