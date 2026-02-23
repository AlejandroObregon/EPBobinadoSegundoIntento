
-- Obtener una configuración de precio por ID
CREATE   PROCEDURE ObtenerConfiguracionPrecio
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cp.Id, cp.PrecioHora, cp.Margen, cp.ConfiguradoPor, cp.FechaConfiguracion,
           u.Nombre AS UsuarioConfigurador, u.Email AS UsuarioEmail
    FROM ConfiguracionPrecios cp
    LEFT JOIN Usuarios u ON cp.ConfiguradoPor = u.Id
    WHERE cp.Id = @Id;
END