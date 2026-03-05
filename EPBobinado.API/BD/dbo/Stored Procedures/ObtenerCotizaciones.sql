
/* =========================================================
   COTIZACIONES
   ========================================================= */
-- Obtener todas las cotizaciones
CREATE PROCEDURE ObtenerCotizaciones
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.Id, c.OrdenId, c.Total, c.Aprobada, c.CreadoEn,
           os.Estado AS OrdenEstado,
           m.NumeroSerie AS MotorNumeroSerie,
           u.Nombre AS UsuarioNombre
    FROM Cotizaciones c
    INNER JOIN OrdenesServicio os ON c.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    ORDER BY c.CreadoEn DESC;
END
GO