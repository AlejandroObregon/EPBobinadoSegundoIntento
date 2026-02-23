
-- Editar un movimiento de inventario existente
CREATE   PROCEDURE EditarMovimientoInventario
    @Id INT,
    @ProductoId INT,
    @OrdenId INT,
    @Tipo NVARCHAR(10),
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Obtener movimiento anterior para ajustar stock
    DECLARE @OldProductoId INT, @OldTipo NVARCHAR(10), @OldCantidad INT;
    SELECT @OldProductoId = ProductoId, @OldTipo = Tipo, @OldCantidad = Cantidad
    FROM MovimientosInventario WHERE Id = @Id;
    
    -- Revertir efecto del movimiento anterior
    IF @OldTipo = 'ENTRADA'
        UPDATE Productos SET Stock = Stock - @OldCantidad WHERE Id = @OldProductoId;
    ELSE IF @OldTipo = 'SALIDA'
        UPDATE Productos SET Stock = Stock + @OldCantidad WHERE Id = @OldProductoId;
    
    -- Actualizar movimiento
    UPDATE MovimientosInventario
    SET ProductoId = @ProductoId,
        OrdenId = @OrdenId,
        Tipo = @Tipo,
        Cantidad = @Cantidad
    WHERE Id = @Id;
    
    -- Aplicar nuevo efecto
    IF @Tipo = 'ENTRADA'
        UPDATE Productos SET Stock = Stock + @Cantidad WHERE Id = @ProductoId;
    ELSE IF @Tipo = 'SALIDA'
        UPDATE Productos SET Stock = Stock - @Cantidad WHERE Id = @ProductoId;
    
    SELECT @Id AS Id;
END