
-- Agregar un nuevo producto
CREATE   PROCEDURE AgregarProducto
    @Nombre NVARCHAR(100),
    @Categoria NVARCHAR(50),
    @Stock INT,
    @StockMinimo INT,
    @Precio DECIMAL(10,2),
    @Activo BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Productos (Nombre, Categoria, Stock, StockMinimo, Precio, Activo)
    VALUES (@Nombre, @Categoria, @Stock, @StockMinimo, @Precio, @Activo);
    
    SELECT SCOPE_IDENTITY() AS Id;
END