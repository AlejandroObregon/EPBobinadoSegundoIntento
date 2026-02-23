
/* =========================================================
   PAGOS
   ========================================================= */

-- Obtener todos los pagos
CREATE   PROCEDURE ObtenerPagos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.Id, p.FacturaId, p.Monto, p.MetodoPago, p.Fecha,
           f.Total AS FacturaTotal,
           os.Estado AS OrdenEstado
    FROM Pagos p
    INNER JOIN Facturas f ON p.FacturaId = f.Id
    INNER JOIN OrdenesServicio os ON f.OrdenId = os.Id
    ORDER BY p.Fecha DESC;
END