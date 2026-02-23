
-- Eliminar un producto
CREATE   PROCEDURE EliminarProducto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Productos WHERE Id = @Id;
    SELECT @Id AS Id;
END