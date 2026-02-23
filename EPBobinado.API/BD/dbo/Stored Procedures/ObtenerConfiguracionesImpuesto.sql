
/* =========================================================
   CONFIGURACIÓN DE IMPUESTOS
   ========================================================= */

-- Obtener todas las configuraciones de impuesto
CREATE   PROCEDURE ObtenerConfiguracionesImpuesto
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ci.Id, ci.Porcentaje, ci.ConfiguradoPor, ci.FechaConfiguracion,
           u.Nombre AS UsuarioConfigurador
    FROM ConfiguracionImpuestos ci
    LEFT JOIN Usuarios u ON ci.ConfiguradoPor = u.Id
    ORDER BY ci.FechaConfiguracion DESC;
END