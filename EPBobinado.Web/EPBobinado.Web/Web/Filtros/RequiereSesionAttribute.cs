using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Filtros
{
    public class RequiereSesionAttribute : Attribute, IPageFilter
    {
        private readonly string _redirectPage;

        public RequiereSesionAttribute(string redirectPage = "/Inicio_de_Sesión/IniciarSesion")
        {
            _redirectPage = redirectPage;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var idUsuario = httpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrEmpty(idUsuario))
            {
                context.Result = new RedirectToPageResult(_redirectPage);
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}

