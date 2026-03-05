/* =========================================================
   FACTURAS
   ========================================================= */
-- Obtener todas las facturas
CREATE PROCEDURE ObtenerFacturas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, f.OrdenId, f.Total, f.Impuesto, f.Fecha,
           os.Estado AS OrdenEstado,
           m.NumeroSerie AS MotorNumeroSerie,
           u.Nombre AS UsuarioNombre
    FROM Facturas f
    INNER JOIN OrdenesServicio os ON f.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Usuarios u ON m.UsuarioId = u.Id
    ORDER BY f.Fecha DESC;
END
GO