
/* =========================================================
   CONFIGURACIÓN DE PRECIOS
   ========================================================= */

-- Obtener todas las configuraciones de precio
CREATE   PROCEDURE ObtenerConfiguracionesPrecio
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cp.Id, cp.PrecioHora, cp.Margen, cp.ConfiguradoPor, cp.FechaConfiguracion,
           u.Nombre AS UsuarioConfigurador
    FROM ConfiguracionPrecios cp
    LEFT JOIN Usuarios u ON cp.ConfiguradoPor = u.Id
    ORDER BY cp.FechaConfiguracion DESC;
END