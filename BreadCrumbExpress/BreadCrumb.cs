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

            //url poderá vir por viewdata ou pelo request
            string url = GetUrlBreadCrumb(html);

            sb.Append("<ul class='navegacao'>");

            if (SiteMap.RootNode.Url.ToLower() == url.ToLower())
            {
                sb.AppendLine("<li class='navegacao selected'>");
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
                        sb.AppendLine("<li class='navegacao selected'>");
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
        
        #endregion       

    
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

            if (url.Contains("Blog/Busca"))
            {
                //~/Blog/Busca/55/7-2011
                url = Path + "Blog/Busca/{id}/{nome}";
            }
            else if (url.Contains("Blog/"))
            {
                //~/Blog/55/Blog do Estrategista
                var urlArr = url.Replace(Path, "/").Split('/');

                //var pastas = ItauHttp.Path.Count(x => x == '/') - 1;

                var id = urlArr[urlArr.Count() - 2].ToString();
                var title = urlArr[urlArr.Count() - 1].ToString();
                url = Path + "Blog/{id}/{nome}";

                html.ViewData.Add("UrlBreadCrumb", string.Format("~/Blog/{0}/{1}", id, title));
                html.ViewData.Add("TitleBreadCrumb", title);
            }
            else if (url.Contains("/PainelCotacoes?viewValue=1"))
            {
                url = url.Replace("?viewValue=1&nomeCarteira=MINHA%20CARTEIRA", "");
            }
            else
            {
                html.ViewData.Add("UrlBreadCrumb", null);
                html.ViewData.Add("TitleBreadCrumb", null);
            }

            return url;
        }
    }
}
