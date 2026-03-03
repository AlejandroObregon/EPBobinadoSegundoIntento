using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.HomePage
{
    public class ConocenosModel : PageModel
    {
        private static readonly List<string> _comentarios = new();

        [BindProperty]
        public string NuevoComentario { get; set; } = string.Empty;

        public IReadOnlyList<string> Comentarios => _comentarios;

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(NuevoComentario))
            {
                _comentarios.Add(NuevoComentario.Trim());
                TempData["OkMsg"] = "¡Gracias por tu comentario!";
            }
            return RedirectToPage();
        }
    }
}
