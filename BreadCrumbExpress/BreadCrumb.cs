using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BreadCrumbExpress
{
    public static class BreadCrumb
    {
        #region BreadCrumb

        private static string ul = "navegacao";

        public static string UlClass
        {
            get
            {
                return ul;
            }
            set
            {
                ul = value;
            }
        }


        private static string Path
        {
            get
            {
                var uri = HttpContext.Current.Request.ApplicationPath;
                if (!uri.EndsWith("/"))
                    uri += "/";
                return uri;
            }
        }

        /// <summary>
        /// Retorna migalha de pão do site
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string BreadCrumbs(System.Web.Mvc.HtmlHelper html)
        {
            var sb = new StringBuilder();
            
            string url = GetUrlBreadCrumb(html);

            sb.Append("<ul class='" + ul + "'>");

            if (SiteMap.RootNode.Url.ToLower() == url.ToLower())
            {
                sb.AppendLine("<li class='" + ul + " selected'>");
                sb.AppendLine(SiteMap.RootNode.Title);
                sb.AppendLine("</li>");
            }
            else
            {
                var todos = SiteMap.RootNode.GetAllNodes();
                SiteMapNode current = null;
                foreach (SiteMapNode item in todos)
                {
                    if (item.Url.ToLower() == url.ToLower())
                    {
                        if (html.ViewData["UrlBreadCrumb"] == null)
                        {
                            current = item;
                        }
                        else
                        {
                            current = new SiteMapNode(item.Provider, item.Key, html.ViewData["UrlBreadCrumb"].ToString(), html.ViewData["TitleBreadCrumb"].ToString());
                        }
                        break;
                    }
                }
                List<SiteMapNode> nodes = new List<SiteMapNode>();
                BuildList(current, ref nodes);
                nodes.Reverse();

                int i = 0;
                foreach (SiteMapNode item in nodes)
                {
                    if (i == nodes.Count - 1)
                    {
                        sb.AppendLine("<li class='" + ul + " selected'>");
                        sb.AppendLine(item.Title);
                    }
                    else
                    {
                        sb.AppendLine("<li>");
                        sb.AppendFormat("<a href='{0}'>{1}</a>", item.Url, item.Title);
                    }
                    sb.AppendLine("</li>");
                    i++;
                }
            }
            sb.AppendLine("</ul>");

            return sb.ToString();
        }
        
        
        public static void BuildList(SiteMapNode node, ref List<SiteMapNode> nodes)
        {
            if (node != null)
            {
                nodes.Add(node);
                BuildList(node.ParentNode, ref nodes);
            }
        }

        public static string GetUrlBreadCrumb(System.Web.Mvc.HtmlHelper html)
        {
            var url = HttpContext.Current.Request.RawUrl.Replace("+", " ");
            html.ViewData.Add("UrlBreadCrumb", null);
            html.ViewData.Add("TitleBreadCrumb", null);
            return url;
        }
    
        #endregion
    }
}
