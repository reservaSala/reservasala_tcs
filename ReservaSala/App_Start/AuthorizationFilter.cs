using ReservaSala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace ReservaSala.App_Start
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        string[] permissoes;
        public AuthorizationFilter(params string[] Permissoes)
        {
            permissoes = Permissoes;

        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //Quando for AllowAnonymousAttribute ele permite acessar independente da autorização
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            int userId = HttpContext.Current.Session["UserID"] == null ? 0 : int.Parse(HttpContext.Current.Session["UserID"].ToString());
            // Check for authorization  
            if (userId == 0)
            {
                filterContext.Result = new RedirectResult("~/UsuarioSalas/Login");
            }

            Models.ReservaSalaContext db = new Models.ReservaSalaContext();

            UsuarioSala usuario = db.UsuarioSalas.FirstOrDefault(u => u.idUsuSala == userId);

            if (usuario == null)
            {
                filterContext.Result = new RedirectResult("~/UsuarioSalas/Login");
                return;
            }

            if (permissoes.Contains(usuario.perfilUsuario))
            {
                return;//tem acesso
            }
            else if (permissoes.Length > 0)
            {
                filterContext.Result = new RedirectResult("~/UsuarioSalas/SemAcesso");
                return;
            }
        }
    }
}