
-- Agregar un nuevo movimiento de inventario
CREATE   PROCEDURE AgregarMovimientoInventario
    @ProductoId INT,
    @OrdenId INT,
    @Tipo NVARCHAR(10),
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO MovimientosInventario (ProductoId, OrdenId, Tipo, Cantidad)
    VALUES (@ProductoId, @OrdenId, @Tipo, @Cantidad);
    
    -- Actualizar stock del producto
    IF @Tipo = 'ENTRADA'
        UPDATE Productos SET Stock = Stock + @Cantidad WHERE Id = @ProductoId;
    ELSE IF @Tipo = 'SALIDA'
        UPDATE Productos SET Stock = Stock - @Cantidad WHERE Id = @ProductoId;
    
    SELECT SCOPE_IDENTITY() AS Id;
END