
-- Editar un producto existente
CREATE   PROCEDURE EditarProducto
    @Id INT,
    @Nombre NVARCHAR(100),
    @Categoria NVARCHAR(50),
    @Stock INT,
    @StockMinimo INT,
    @Precio DECIMAL(10,2),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Productos
    SET Nombre = @Nombre,
        Categoria = @Categoria,
        Stock = @Stock,
        StockMinimo = @StockMinimo,
        Precio = @Precio,
        Activo = @Activo
    WHERE Id = @Id;
    
    SELECT @Id AS Id;
END