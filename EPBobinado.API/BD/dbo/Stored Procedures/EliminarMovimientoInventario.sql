
-- Eliminar un movimiento de inventario
CREATE   PROCEDURE EliminarMovimientoInventario
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Obtener movimiento para ajustar stock
    DECLARE @ProductoId INT, @Tipo NVARCHAR(10), @Cantidad INT;
    SELECT @ProductoId = ProductoId, @Tipo = Tipo, @Cantidad = Cantidad
    FROM MovimientosInventario WHERE Id = @Id;
    
    -- Revertir efecto en stock
    IF @Tipo = 'ENTRADA'
        UPDATE Productos SET Stock = Stock - @Cantidad WHERE Id = @ProductoId;
    ELSE IF @Tipo = 'SALIDA'
        UPDATE Productos SET Stock = Stock + @Cantidad WHERE Id = @ProductoId;
    
    DELETE FROM MovimientosInventario WHERE Id = @Id;
    SELECT @Id AS Id;
END