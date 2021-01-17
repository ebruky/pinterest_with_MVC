using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunucuMvcPinterest.Models
{
    public class UserAuthorize : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (HttpContext.Current.Session["id"] == null)
            {
                httpContext.Response.Redirect("~/Home/Giris");
            }
            else
            {
                
                string Id = HttpContext.Current.Session["id"].ToString();
                int id = int.Parse(Id);

                if (id != 1)
                {
                    return true;
                }

            }
            return base.AuthorizeCore(httpContext);
        }

    }
}