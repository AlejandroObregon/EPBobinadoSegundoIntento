using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Servicios;
using Microsoft.Extensions.Configuration;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private IConfiguration _configuration;

        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerMetodo(string seccion, string nombre)
        {
            var config = _configuration.GetSection(seccion).Get<APIEndPoint>();

            if (config == null)
                throw new Exception($"No se encontró la sección '{seccion}' en appsettings.json.");

            var metodo = config.Metodos.FirstOrDefault(m => m.Nombre == nombre);

            if (metodo == null)
                throw new Exception($"No se encontró el método '{nombre}' en la sección '{seccion}'.");

            var baseUrl = config.UrlBase?.TrimEnd('/');
            var path = metodo.Valor?.TrimStart('/');

            return $"{baseUrl}/{path}";
        }


        private string? ObtenerUrlBase(string seccion)
        {
            return _configuration.GetSection(seccion).Get<APIEndPoint>().UrlBase;
        }

        public string ObtenerValor(string llave)
        {
            return _configuration.GetSection(llave).Value;
        }
    }
}
