
/* =========================================================
   MOVIMIENTOS DE INVENTARIO
   ========================================================= */

-- Obtener todos los movimientos de inventario
CREATE   PROCEDURE ObtenerMovimientosInventario
AS
BEGIN
    SET NOCOUNT ON;
    SELECT mi.Id, mi.ProductoId, mi.OrdenId, mi.Tipo, mi.Cantidad, mi.Fecha,
           p.Nombre AS ProductoNombre,
           os.Estado AS OrdenEstado
    FROM MovimientosInventario mi
    INNER JOIN Productos p ON mi.ProductoId = p.Id
    LEFT JOIN OrdenesServicio os ON mi.OrdenId = os.Id
    ORDER BY mi.Fecha DESC;
END