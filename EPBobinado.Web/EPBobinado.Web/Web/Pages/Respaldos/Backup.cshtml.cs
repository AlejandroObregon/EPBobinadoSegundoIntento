using Abstracciones.Interfaces.Reglas;
using Microsoft.AspNetCore.Mvc;
using Web.Pages;

namespace Web.Pages.Backup
{
    public class BackupModel : PageModelBase
    {
        private readonly IConfiguracion _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public BackupModel(IConfiguracion config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        // GET normal → mostrar página
        public IActionResult OnGet()
        {
            var auth = VerificarSesion(2);
            if (auth != null) return auth;
            return Page();
        }

        // GET ?handler=Bak → descarga .bak
        public async Task<IActionResult> OnGetBakAsync()
        {
            var auth = VerificarSesion(2);
            if (auth != null) return auth;
            return await DescargarAsync("Bak", "application/octet-stream", "bak");
        }

        // GET ?handler=Sql → descarga .sql
        public async Task<IActionResult> OnGetSqlAsync()
        {
            var auth = VerificarSesion(2);
            if (auth != null) return auth;
            return await DescargarAsync("Sql", "application/sql", "sql");
        }

        private async Task<IActionResult> DescargarAsync(string tipo, string mimeType, string ext)
        {
            try
            {
                var url = _config.ObtenerMetodo("ApiEndPointsBackup", tipo);
                var client = _httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromMinutes(6);

                var resp = await client.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                {
                    var body = await resp.Content.ReadAsStringAsync();
                    TempData["Error"] = $"Error {(int)resp.StatusCode}: {body}";
                    return Redirect("/Respaldos/Backup");
                }

                var bytes = await resp.Content.ReadAsByteArrayAsync();
                var filename = resp.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                               ?? $"FitnescoreDB_{DateTime.Now:yyyyMMdd_HHmmss}.{ext}";

                return File(bytes, mimeType, filename);
            }
            catch (TaskCanceledException)
            {
                TempData["Error"] = "El backup tardó demasiado. Intentá de nuevo o usá SSMS.";
                return Redirect("/Respaldos/Backup");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error inesperado: {ex.Message}";
                return Redirect("/Respaldos/Backup");
            }
        }
    }
}