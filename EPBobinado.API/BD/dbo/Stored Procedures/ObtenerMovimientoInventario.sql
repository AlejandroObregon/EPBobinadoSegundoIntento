
-- Obtener un movimiento de inventario por ID
CREATE   PROCEDURE ObtenerMovimientoInventario
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT mi.Id, mi.ProductoId, mi.OrdenId, mi.Tipo, mi.Cantidad, mi.Fecha,
           p.Nombre AS ProductoNombre, p.Categoria AS ProductoCategoria,
           os.Estado AS OrdenEstado
    FROM MovimientosInventario mi
    INNER JOIN Productos p ON mi.ProductoId = p.Id
    LEFT JOIN OrdenesServicio os ON mi.OrdenId = os.Id
    WHERE mi.Id = @Id;
END