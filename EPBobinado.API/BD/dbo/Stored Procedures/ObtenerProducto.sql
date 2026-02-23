
-- Obtener un producto por ID
CREATE   PROCEDURE ObtenerProducto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Categoria, Stock, StockMinimo, Precio, Activo
    FROM Productos
    WHERE Id = @Id;
END