
-- Obtener una configuración de impuesto por ID
CREATE   PROCEDURE ObtenerConfiguracionImpuesto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ci.Id, ci.Porcentaje, ci.ConfiguradoPor, ci.FechaConfiguracion,
           u.Nombre AS UsuarioConfigurador, u.Email AS UsuarioEmail
    FROM ConfiguracionImpuestos ci
    LEFT JOIN Usuarios u ON ci.ConfiguradoPor = u.Id
    WHERE ci.Id = @Id;
END