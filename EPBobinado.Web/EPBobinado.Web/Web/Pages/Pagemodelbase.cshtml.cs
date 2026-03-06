using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    /// <summary>
    /// Clase base para todos los PageModels que requieren sesión activa.
    /// Si no hay sesión, redirige automáticamente a /Auth/Login.
    /// </summary>
    public abstract class PageModelBase : PageModel
    {
        /// <summary>Id del usuario en sesión (0 si no hay sesión).</summary>
        public int UsuarioId { get; protected set; }

        /// <summary>RolId del usuario en sesión (0 si no hay sesión).</summary>
        public int UsuarioRol { get; protected set; }

        /// <summary>Nombre del usuario en sesión.</summary>
        public string UsuarioNombre { get; protected set; } = "";

        /// <summary>
        /// Verifica la sesión. Llama esto al inicio de OnGetAsync / OnPostAsync.
        /// Devuelve un RedirectResult si no hay sesión, o null si todo está bien.
        /// </summary>
        protected IActionResult? VerificarSesion()
        {
            var idStr = HttpContext.Session.GetString("UsuarioId");

            if (string.IsNullOrWhiteSpace(idStr))
                return Redirect("/Auth/Login");

            int.TryParse(idStr, out int uid);
            int.TryParse(HttpContext.Session.GetString("UsuarioNivel"), out int rol);

            if (uid == 0)
                return Redirect("/Auth/Login");

            UsuarioId = uid;
            UsuarioRol = rol;
            UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre") ?? "";
            return null;
        }

        /// <summary>
        /// Igual que VerificarSesion pero además valida que el rol sea uno de los permitidos.
        /// Útil para rutas exclusivas de admin (rol 2) o técnico (rol 1002).
        /// </summary>
        protected IActionResult? VerificarSesion(params int[] rolesPermitidos)
        {
            var redirect = VerificarSesion();
            if (redirect != null) return redirect;

            if (rolesPermitidos.Length > 0 && !rolesPermitidos.Contains(UsuarioRol))
                return Redirect("/ClienteHome"); // o una página de "acceso denegado"

            return null;
        }
    }
}