using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class MenuHelper
{
    public static MvcHtmlString MenuLink(this HtmlHelper html,
            string linkText, string actionName,
            string controllerName)
    {
        string currentAction = html.ViewContext.RouteData.GetRequiredString("action");
        string currentController = html.ViewContext.RouteData.GetRequiredString("controller");

        TagBuilder tag = new TagBuilder("li");
        tag.InnerHtml = html.ActionLink(linkText, actionName, controllerName).ToHtmlString();
        if (controllerName == currentController && actionName == currentAction)
        {
            tag.AddCssClass("active");
        }
        return MvcHtmlString.Create(tag.ToString());
    }
}