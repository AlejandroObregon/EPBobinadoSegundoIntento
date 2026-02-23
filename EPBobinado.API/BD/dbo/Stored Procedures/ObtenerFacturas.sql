
/* =========================================================
   FACTURAS
   ========================================================= */

-- Obtener todas las facturas
CREATE   PROCEDURE ObtenerFacturas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.Id, f.OrdenId, f.Total, f.Impuesto, f.Fecha,
           os.Estado AS OrdenEstado,
           m.NumeroSerie AS MotorNumeroSerie,
           cl.Nombre AS ClienteNombre
    FROM Facturas f
    INNER JOIN OrdenesServicio os ON f.OrdenId = os.Id
    INNER JOIN Motores m ON os.MotorId = m.Id
    INNER JOIN Clientes cl ON m.ClienteId = cl.Id
    ORDER BY f.Fecha DESC;
END